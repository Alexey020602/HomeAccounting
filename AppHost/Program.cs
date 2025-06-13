using Microsoft.Extensions.DependencyInjection;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.AddDockerComposeEnvironment("docker");
var db = builder
    .AddPostgres("db")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("HomeAccounting");

var migrations = builder
    .AddProject<MigrationService>("migrations")
    
    .WithReference(db)
    .WaitFor(db);

var api = builder
    .AddProject<Api>("api")
    .WithExternalHttpEndpoints()
    .WaitForCompletion(migrations)
    .WithReference(db)
    .WithHttpHealthCheck("/health");
;

var client = builder.AddProject<Client>("client")
    .WaitFor(api)
    ;

var serverClient = builder.AddProject<WebClient>("webclient")
    .WaitFor(api);

var yarp = builder
        .AddProject<Gateway>("gateway")
        // .AddYarp("apigateway")
        // .WithConfigFile("yarp.json")
        .WithReference(api)
        .WithReference(client)
        .WithExternalHttpEndpoints()
    ;

builder.Build().Run();