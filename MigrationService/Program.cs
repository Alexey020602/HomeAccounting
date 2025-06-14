using Authorization;
using Authorization.DependencyInjection;
using Checks.Contracts;
using Checks.Core;
using Checks.DataBase;
using Fns;
using Fns.Contracts;
using Microsoft.EntityFrameworkCore;
using MigrationService;
using Refit;
using Shared.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging();
builder.Services.AddHttpLogging();
builder.AddServiceDefaults();

builder.AddDbContexts();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();