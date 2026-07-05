using Microsoft.EntityFrameworkCore;
using Octokit;
using ImageGallery.Infrastructure.Data;
using ImageGallery.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Supabase:AuthUrl"];
        options.Audience = "authenticated";
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
    });

builder.Services.AddAuthorization();

var productHeader = new ProductHeaderValue("ImageGallery", "1.0");
builder.Services.AddSingleton(new GitHubClient(productHeader));

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IGitHubImportService, GitHubImportService>();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
