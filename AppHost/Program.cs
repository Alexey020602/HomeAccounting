using Aspire.Hosting.Docker.Resources.ComposeNodes;
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

builder.Services.AddHealthChecks()
    .AddNpgSql();


var db = builder
    .AddPostgres("db")
    .WithUserName(username)
    .WithPassword(password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHealthCheck("npgsql")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("HomeAccounting");

var api = builder
    .AddProject<Api>("api")
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService((resource, service) =>
    {
        // service.DependsOn.Add("db", new ServiceDependency(){ Condition = "service_healthy"}); 
        // service.Ports.Add("80:${API_PORT}");
    })
    .WithReference(db)
    .WaitFor(db)
    .WaitForCompletion(db)
    .WithHttpHealthCheck("/health");
;

builder.Build().Run();