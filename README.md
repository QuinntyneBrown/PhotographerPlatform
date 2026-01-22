# PhotographerPlatform

PhotographerPlatform is a multi-service platform for running a photography business: client management, project workflow, gallery delivery, online store, portfolio websites, billing, communications, analytics, and support.

## Solution layout

- `src/PhotographerPlatform.ApiGateway` - API gateway for routing/aggregation.
- `src/Identity` - authentication/identity services (targets `net8.0`).
- `src/PhotographerPlatform.*` - bounded-context services, each split into `Api`, `Core`, and `Infrastructure` projects (targets `net9.0`).
  - Analytics, Billing, Communication, Galleries, Integrations, Security, Store, Support, Websites, Workspace.
- `src/Shared` - shared contracts/utilities for services.
- `src/PhotographerPlatform.Ui` - Angular admin UI.
- `tests` - xUnit test projects for services and shared code.
- `docs` - requirements, coding guidelines, and admin UI implementation guide.

## Tech stack

- .NET microservices (primarily `net9.0`, Identity on `net8.0`)
- Angular 21 admin UI
- xUnit for .NET tests

## Getting started

### Backend services

```powershell
dotnet restore
dotnet build
dotnet test
```

Run a service locally (example):

```powershell
dotnet run --project src\PhotographerPlatform.ApiGateway\PhotographerPlatform.ApiGateway.csproj
```

### Admin UI

```powershell
cd src\PhotographerPlatform.Ui
npm install
npm run start
```

## Documentation

- Requirements: `docs/requirements.md`
- Coding guidelines: `docs/coding-guidelines.md`
- Admin UI guide: `docs/ADMIN-UI-IMPLEMENTATION-GUIDE.md`
- Roadmap: `docs/roadmap`
