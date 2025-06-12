using Checks.Contracts;
using Checks.Core;
using Checks.DataBase;
using Fns;
using Fns.Contracts;
using Microsoft.EntityFrameworkCore;
using MigrationService;
using Refit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging();
builder.Services.AddHttpLogging();
builder.AddServiceDefaults();
//     .AddDefaultHealthChecks()
//     .ConfigureOpenTelemetry();
// builder.Services.AddTransient<HttpLoggingHandler>();

builder.Services.AddFnsModule();

builder.Services.AddScoped<ICheckRepository, CheckRepository>();
builder.Services.AddScoped<ICheckUseCase, CheckUseCase>();
builder.Services.AddTransient<ApplicationContextSeed>();

if (builder.Environment.IsDevelopment())
    builder.AddNpgsqlDbContext<ChecksContext>("HomeAccounting");
else
    builder.Services.AddDbContext<ChecksContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();