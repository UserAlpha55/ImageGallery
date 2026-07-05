---
name: ef-core-migration
description: Reference for EF Core migration commands specific to ImageGallery's PostgreSQL setup
---

## ImageGallery migration commands

All migrations target `ImageGallery.Infrastructure` with `ImageGallery.API` as the startup project.

### Create a migration
```bash
dotnet ef migrations add <MigrationName> --project src/ImageGallery.Infrastructure --startup-project src/ImageGallery.API
```

### Apply migrations to database
The API runs `db.Database.Migrate()` automatically on startup in `Program.cs`.

### Remove last migration (if not applied)
```bash
dotnet ef migrations remove --project src/ImageGallery.Infrastructure --startup-project src/ImageGallery.API
```

### Generate SQL script
```bash
dotnet ef migrations script --project src/ImageGallery.Infrastructure --startup-project src/ImageGallery.API --output script.sql
```

### List migrations
```bash
dotnet ef migrations list --project src/ImageGallery.Infrastructure --startup-project src/ImageGallery.API
```

### Important notes
- Provider: Npgsql (PostgreSQL) — SQLite provider is no longer used
- Connection string is read from `appsettings.json` or `appsettings.Development.json`
- Migration files are in `src/ImageGallery.Infrastructure/Data/Migrations/`
- Migrations are gitignored by `**/Migrations/*.cs` rule (only snapshot is tracked)
