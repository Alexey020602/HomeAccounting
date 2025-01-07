var builder = DistributedApplication.CreateBuilder(args);

var api = builder
    .AddProject<Projects.WebApplication1>("api")
    // .WithExternalHttpEndpoints()
    ;

builder.AddProject<Projects.Client>("client")
    // .WithReference(api)
    // .WaitFor(api)
    // .WithEnvironment("ApiUrlHttp", api.GetEndpoint("http"))
    // .WithEnvironment("ApiUrlHttps", api.GetEndpoint("https"))
    ;

builder.Build().Run();