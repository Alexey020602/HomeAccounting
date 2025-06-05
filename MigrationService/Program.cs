using Checks.Core;
using Core;
using Core.Services;
using Checks.DataBase;
using Fns;
using Microsoft.EntityFrameworkCore;
using MigrationService;
using Refit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<ApplicationContextSeed>();
builder.Services.AddLogging();

builder.Services.AddTransient<HttpLoggingHandler>();

builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"))
    .AddHttpMessageHandler<HttpLoggingHandler>();
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"))
    .AddHttpMessageHandler<HttpLoggingHandler>();

builder.Services.AddScoped<ICheckRepository, CheckRepository>();
// builder.Services.AddScoped<ICheckSource, CheckSource>();
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