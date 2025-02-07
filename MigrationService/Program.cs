using Core;
using Core.Services;
using DataBase;
using FnsChecksApi;
using Microsoft.EntityFrameworkCore;
using MigrationService;
using Refit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<ApplicationContextSeed>();
builder.Services.AddLogging();
builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"));

builder.Services.AddScoped<ICheckUseCase, CheckUseCase>();
builder.Services.AddTransient<ICheckUseCase, CheckUseCase>();
if (builder.Environment.IsDevelopment())
    builder.AddNpgsqlDbContext<ApplicationContext>("HomeAccounting");
else
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();