
# GitHub Copilot Instructions (auto-generated)

**Last updated**: 2025-10-31

## Architecture Overview

- Monorepo with four main projects:
	- `Sudoku.Api`: ASP.NET Core Web API (REST endpoints, JWT auth, Swagger docs)
	- `Sudoku.Core`: Domain models, interfaces, business logic
	- `Sudoku.Infrastructure`: Data access (EF Core, Npgsql), repository/service implementations, migrations
	- `Sudoku.UI`: Blazor WebAssembly frontend (Material/MudBlazor planned), connects to API via `ApiService`

## Developer Workflows

- **Build & Test**:
	- `dotnet build` and `dotnet test` from `src/`
	- Solution file: `src/Sudoku.sln`
- **Run API**:
	- `dotnet run` from `src/Sudoku.Api`
	- API available at `https://localhost:5001` (Swagger UI at `/swagger`)
- **Run UI**:
	- `dotnet run` from `src/Sudoku.UI`
	- UI at `https://localhost:7001`
- **Database**:
	- PostgreSQL via Docker Compose (`docker compose up -d`)
	- EF Core migrations:
		- Add: `dotnet ef migrations add <Name> --project Sudoku.Infrastructure --startup-project Sudoku.Api`
		- Update: `dotnet ef database update --project Sudoku.Infrastructure --startup-project Sudoku.Api`
- **DevContainer**:
	- VS Code Dev Containers supported; auto-setup for .NET, PostgreSQL, NuGet restore, build

## Key Patterns & Conventions

- **Board state**: Stored as JSONB arrays in PostgreSQL; use GIN indices for querying
- **Authentication**: JWT Bearer; session endpoints require auth
- **API contracts**: OpenAPI spec at `specs/1-sudoku-platform/contracts/openapi.yaml`
- **Error handling**: Middleware in `Sudoku.Api/Middleware/`
- **Entity/Repository/Service**:
	- Entities: `Sudoku.Core/Entities/`
	- Repositories: Interfaces in `Sudoku.Core/Repositories/`, implementations in `Sudoku.Infrastructure/Repositories/`
	- Services: Interfaces in `Sudoku.Core/Services/`, implementations in `Sudoku.Infrastructure/Services/`
- **Frontend**:
	- Pages/components in `Sudoku.UI/Pages/` and `Sudoku.UI/Layout/`
	- API integration via `Sudoku.UI/Services/ApiService.cs`
	- Planned: Material UI via MudBlazor

## Integration Points

- **Swagger/OpenAPI**: Interactive docs at API root
- **Postman**: Collection at `docs/postman/Sudoku-API.postman_collection.json`
- **Environment config**: `.env.sample` → `.env` for secrets, DB, JWT

## Testing

- **Unit tests**:
	- Core logic: `tests/Unit/Sudoku.Core.Tests/`
	- UI/service: `tests/Unit/Sudoku.UI.Tests/`
- **Integration tests**:
	- API flows: `tests/Integration/Sudoku.Integration.Tests/`
	- Planned UI flows: `tests/Integration/Sudoku.UI.IntegrationTests/`

## Recent Changes

- Added OpenAPI contract and plan artifacts for backend
- UI tasks tracked in `specs/2-fancy-ui/tasks.md` (strict checklist format)

---

**Feedback requested:**
- Are any architectural details unclear?
- Is there a pattern or workflow not covered here?
- Should any file or convention be described in more detail?

Let me know if you want to iterate or expand any section.
