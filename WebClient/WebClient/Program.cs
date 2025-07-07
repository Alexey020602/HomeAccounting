using Shared.Blazor.Layouts;
using WebClient.Components;

var builder = WebApplication.CreateBuilder(args);
//
//
// builder.Services.AddRazorComponents()
//     .AddInteractiveWebAssemblyComponents();
//
// builder.Services.Configure<RouteOptions>(options =>
// {
//     options.SuppressCheckForUnhandledSecurityMetadata = true;
// });
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseWebAssemblyDebugging();
// }
// else
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
//
// app.UseAntiforgery();
//
// app.MapStaticAssets();
//
// app.MapRazorComponents<App>()
//     .AddInteractiveWebAssemblyRenderMode()
//     // .AddAdditionalAssemblies(typeof(Client._Imports).Assembly)
//     .AddAdditionalAssemblies(typeof(MainLayout).Assembly)
//     // .AddAdditionalAssemblies(typeof(Client.App).Assembly)
//     // .AddAdditionalAssemblies(typeof(WebClient.Client._Imports).Assembly)
//     ;
//
// app.Run();