---
name: supabase-setup
description: Reference guide for getting Supabase project credentials from the dashboard
---

## Getting your Supabase credentials

1. Go to https://supabase.com → sign in → open your project
2. **Database connection string**: Project Settings → Database → Connection string (URI format: `Host=db.X.supabase.co; Database=postgres; Username=postgres; Password=XXX; SSL Mode=Require`)
3. **Auth URL**: Project Settings → API → Project URL + `/auth/v1`
4. **Anon key**: Project Settings → API → Project API keys → `anon public`
5. **Service role key**: Project Settings → API → Project API keys → `service_role` (keep secret)

## Where to put them

Create `src/ImageGallery.API/appsettings.Development.json` (gitignored):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.YOUR-PROJECT.supabase.co; Database=postgres; Username=postgres; Password=YOUR-PASSWORD; SSL Mode=Require"
  },
  "Supabase": {
    "AuthUrl": "https://YOUR-PROJECT.supabase.co/auth/v1",
    "AnonKey": "YOUR-ANON-KEY",
    "ServiceRoleKey": "YOUR-SERVICE-ROLE-KEY"
  }
}
```

The committed `appsettings.json` has `YOUR-*` placeholders. Real values live in Development config only.
