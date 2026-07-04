using ImageGallery.Web.Components;
using ImageGallery.Client.Services;
using ImageGallery.Web.App.Services;
using MudBlazor.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddMudServices();

builder.Services.AddScoped<AuthStateService>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5231";

builder.Services.AddHttpClient<IGalleryClient, GalleryClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler(sp =>
{
    var authState = sp.GetRequiredService<AuthStateService>();
    return new AuthTokenHandler(authState);
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

public class AuthTokenHandler : DelegatingHandler
{
    private readonly AuthStateService _authState;

    public AuthTokenHandler(AuthStateService authState) => _authState = authState;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        if (_authState.IsAuthenticated)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authState.AccessToken);
        return await base.SendAsync(request, ct);
    }
}
