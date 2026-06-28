using ImageGallery.Web.Components;
using ImageGallery.Client.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddMudServices();

//var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001";
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5231";

builder.Services.AddHttpClient<IGalleryClient, GalleryClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
