using Yarp.ReverseProxy.Configuration;

IReadOnlyList<RouteConfig> routes = [
    new RouteConfig()
    {
        RouteId = "clientRoute",
        ClusterId = "client",
        Match = new()
        {
            Path = "/{**catch-all}",
        },
    },
    new()
    {
        RouteId = "apiRoute",
        ClusterId = "api",
        Match = new()
        {
            Path = "/api/{**catch-all}",
        },
        Transforms =
        [
            new Dictionary<string, string>
            {
                { "PathPattern", "/{**catch-all}" },
            }
        ]
    }
];
IReadOnlyList<ClusterConfig> clusters = [
    new ClusterConfig()
    {
        ClusterId = "client",
        Destinations = new Dictionary<string, DestinationConfig>()
        {
            {
                "client/d1",
                new()
                {
                    Address = "http://client"
                }
            }
        }
    },
    new ClusterConfig()
    {
        ClusterId = "api",
        Destinations = new Dictionary<string, DestinationConfig>()
        {
            {
                "api/d1",
                new DestinationConfig()
                {
                    Address = "http://api"
                }
            }
        }
    }
];

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes,
        clusters)
    .AddServiceDiscoveryDestinationResolver();
builder.Services.AddLettuceEncrypt();

var app = builder.Build();
app.MapReverseProxy();

app.Run();

