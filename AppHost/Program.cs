using Aspire.Hosting.Docker.Resources.ComposeNodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aspire.Hosting.Postgres;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.AddDockerComposeEnvironment("docker");

var databaseConfigurationSection = builder.Configuration.GetSection("Database");
var username = builder.AddParameter("Username", databaseConfigurationSection.GetValue<string>("username")!, secret: true);
var password = builder.AddParameter("Password",databaseConfigurationSection.GetValue<string>("password")!, secret: true);
// IResourceBuilder<ParameterResource> username = new  "home_accounting_user";
// IResourceBuilder<ParameterResource> password = "654765as465gf4as";
var db = builder
    .AddPostgres("db")
    .WithUserName(username)
    .WithPassword(password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("HomeAccounting");

// var migrations = builder
//     .AddProject<MigrationService>("migrations")
//     .WithReference(db)
//     .WaitFor(db);

var api = builder
    .AddProject<Api>("api")
    .WithExternalHttpEndpoints()
    // .WaitForCompletion(migrations)
    .WithReference(db)
    .WithHttpHealthCheck("/health");
;

// var client = builder.AddProject<Client>("client")
//     .WaitFor(api)
//     ;

// var client = builder.AddProject<WebClient>("client")
//     .WaitFor(api);
//
// var yarp = builder
//         .AddProject<Gateway>("gateway")
//         // .AddYarp("apigateway")
//         // .WithConfigFile("yarp.json")
//         .WithReference(api)
//         .WithReference(client)
//         .WithExternalHttpEndpoints()
//     ;

builder.Build().Run();