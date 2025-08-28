using Aspire.Hosting.Docker.Resources.ComposeNodes;
using Aspire.Hosting.Docker.Resources.ServiceNodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aspire.Hosting.Postgres;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.AddDockerComposeEnvironment("docker")
    .WithDashboard(dashboard =>
    {
        dashboard.PublishAsDockerComposeService((_, service) =>
        {
            service.Expose.Add("11111");
            service.Ports.Add("11111:11111");
        });
    });

var username = builder.AddParameter("Username", secret: true);
var password = builder.AddParameter("Password", secret: true);

var db = builder
    .AddPostgres("db")
    .WithUserName(username)
    .WithPassword(password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin()
    
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Healthcheck = new Healthcheck()
        {
            Test = [
                "[",
                "CMD", 
                "pg_isready",
                "-U ${USERNAME}",
                "-d HomeAccounting",
                "]"
            ],
            Interval = "10s",
            Timeout = "5s",
            Retries = 5,
            StartPeriod = "20s"
        };
    })
    .AddDatabase("HomeAccounting");

var api = builder
    .AddProject<Api>("api")
    // .WithHttpEndpoint()
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService((resource, service) =>
    {
        if (builder.ExecutionContext.IsPublishMode)
        {
            service.DependsOn.Add("db", new ServiceDependency() { Condition = "service_healthy"});
        }
    })
    .WithReference(db)
    .WithHttpHealthCheck("/health");
if (!builder.ExecutionContext.IsPublishMode)
{
    api = api
            .WaitFor(db);
}

builder.Build().Run();