# ImageGallery

## Stack
- .NET 10, ASP.NET Core
- Blazor Interactive Server + MudBlazor 9.6
- EF Core 10 (Npgsql) → Supabase PostgreSQL
- Supabase Auth (JWT Bearer)
- Clean Architecture (8 projects)

## Key files
- `src/ImageGallery.API/Program.cs` — API startup + DI
- `src/ImageGallery.Infrastructure/Data/AppDbContext.cs` — EF Core context
- `src/ImageGallery.Web/Program.cs` — Blazor host startup
- `src/ImageGallery.Web.App/Services/AuthStateService.cs` — Auth state
- `src/ImageGallery.Client/Services/GalleryClient.cs` — API client

## Commands
- Build: `dotnet build`
- Migration: `dotnet ef migrations add <name> --project src/ImageGallery.Infrastructure --startup-project src/ImageGallery.API`
- Run API: `dotnet run --project src/ImageGallery.API`
- Run Web: `dotnet run --project src/ImageGallery.Web`

## Rules
- All API controllers need `[Authorize]`
- Entity Id = Guid
- Never commit real credentials — use `appsettings.Development.json`
- Never commit, push, or create PRs unless explicitly asked
- Follow Claude Code directory structure for agents, skills, and MCP configurations
