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

var username = builder.AddParameter("Username", secret: true, value: "myuser");
var password = builder.AddParameter("Password", secret: true, value: "sdadfgsqafasdfas");

var db = builder
    .AddPostgres("db", username, password)
    .WithUserName(username)
    .WithPassword(password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("HomeAccounting");

// var api = builder
//     .AddProject<Api>("api")
//     .WithExternalHttpEndpoints()
//     .WithReference(db)
//     .WithHttpHealthCheck("/health");

var myBudgets = builder.AddProject<MyBudgets>("mybudgets")
        .WithReference(db)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    ;



builder.Build().Run();