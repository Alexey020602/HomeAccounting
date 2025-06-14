using BlazorShared.DependencyInjection;
using MudBlazor.Extensions;
using WebClient.Components;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration.GetSection("Api").GetValue<string>("ApiUrlHttps") ?? throw new Exception("Отсутствует адрес для api");
var apiUri = new Uri(apiUrl);
builder.Services.AddBlazorShared(apiUri);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<Client.App>()
    .AddAdditionalAssemblies(typeof(Program).Assembly)
    .AddInteractiveWebAssemblyRenderMode()
    // .AddAdditionalAssemblies(typeof(Client.App).Assembly)
    // .AddAdditionalAssemblies(typeof(WebClient.Client._Imports).Assembly)
    ;

app.Run();