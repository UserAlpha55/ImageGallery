---
name: deployment
description: Checklist for deploying ImageGallery .NET app to a hosting provider
---

## Deployment checklist

### Prerequisites
- [ ] Supabase project is active with correct connection string
- [ ] Supabase Auth is enabled with GitHub OAuth configured
- [ ] EF Core migrations are generated and committed (or generated at deploy time)

### Hosting options

| Provider | Free tier | Notes |
|---|---|---|
| MonsterASP.NET | Yes | Traditional IIS, Web Deploy |
| Render | Yes (sleeps on idle) | Docker or native .NET |
| Railway | $5 credit | Easy .NET support |
| Fly.io | 3 VMs free | Native .NET support |

### Environment variables (set on hosting provider)
| Variable | Value |
|---|---|
| `ConnectionStrings__DefaultConnection` | Your Supabase PostgreSQL connection string |
| `Supabase__AuthUrl` | `https://YOUR-PROJECT.supabase.co/auth/v1` |
| `Supabase__AnonKey` | Your anon public key |
| `Supabase__ServiceRoleKey` | Your service role key |
| `ApiBaseUrl` (Web app) | URL of the deployed API |

### After deployment
- [ ] Verify API health at `/swagger`
- [ ] Verify Blazor UI loads and connects to API
- [ ] Test GitHub login flow end-to-end
- [ ] Check that EF Core migrations applied on startup
