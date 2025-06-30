using MigrationService;
using Shared.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging();
builder.Services.AddHttpLogging();
builder.AddServiceDefaults();

builder.AddDbContexts();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();