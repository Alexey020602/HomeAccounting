var builder = DistributedApplication.CreateBuilder(args);

var db = builder
    .AddPostgres("db")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("HomeAccounting");

var migrations = builder
    .AddProject<Projects.MigrationService>("migrations")
    .WithReference(db)
    .WaitFor(db);

var api = builder
    .AddProject<Projects.Api>("api")
    .WithExternalHttpEndpoints()
    // .WithHttpEndpoint()
    // .WithHttpsEndpoint()
    .WaitForCompletion(migrations)
    .WithReference(db);
    // .WithExternalHttpEndpoints()
    ;

builder.AddProject<Projects.Client>("client")
    // .WithExternalHttpEndpoints()
    // .WithHttpEndpoint(port: 5000, targetPort: 5160)
    // .WithHttpsEndpoint()
    // .WithReference(api)
    // .WaitFor(api)
    // .WithEnvironment("ApiUrlHttp", api.GetEndpoint("http"))
    // .WithEnvironment("ApiUrlHttps", api.GetEndpoint("https"))
    ;

builder.Build().Run();