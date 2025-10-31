
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

## Code Quality & Formatting

- **No explicit linter**: Project currently uses standard .NET conventions
- **Formatting**: Follow C# coding conventions (PascalCase for public members, camelCase for private fields)
- **Analyzers**: .NET SDK built-in analyzers are active during build
- **Code style**: Consistent with existing code in the repository
- **Naming conventions**:
	- Controllers: `{Entity}Controller` (e.g., `PuzzlesController`)
	- Services: `{Entity}Service` (e.g., `PuzzleService`)
	- Repositories: `{Entity}Repository` (e.g., `PuzzleRepository`)
	- Entities: Singular noun (e.g., `Puzzle`, `User`, `Session`)

## Security Considerations

- **Secrets**: NEVER commit secrets to source code
	- Use `.env` file for local development (excluded from git via `.gitignore`)
	- Environment variables for production
	- JWT secrets must be 32+ characters
- **Authentication**: All session endpoints require JWT Bearer token
- **Database**: Use parameterized queries (EF Core handles this automatically)
- **HTTPS**: Required for production; development uses self-signed certificate
- **Password validation**: Enforced via ASP.NET Core Identity defaults

## CI/CD Workflows

- **GitHub Actions**: `.github/workflows/dotnet.yml`
	- Triggers: Push to `main` or `copilot/**` branches, PRs to `main`
	- PostgreSQL service container auto-configured for tests
	- Steps: Restore → Build → Unit Tests → Integration Tests
- **Build configuration**: Use `Release` for CI/CD
- **Test requirements**: All tests must pass before merge

## Dependency Management

- **Package manager**: NuGet (via `dotnet` CLI)
- **Restore**: `dotnet restore src/Sudoku.sln`
- **Update packages**: Use `dotnet add package` or edit `.csproj` files
- **Key dependencies**:
	- ASP.NET Core 9.0
	- Entity Framework Core 9.0
	- Npgsql.EntityFrameworkCore.PostgreSQL
	- Swashbuckle.AspNetCore (Swagger/OpenAPI)
	- xUnit (testing)

## Common Issues & Troubleshooting

- **Build fails with "Assets file not found"**: Run `dotnet restore src/Sudoku.sln`
- **Database connection fails**: 
	- Check PostgreSQL is running: `docker compose ps`
	- Start database: `docker compose up -d`
	- Verify connection string in `.env` matches `docker-compose.yml`
- **Migration errors**: 
	- Ensure PostgreSQL is running
	- Check connection string
	- Verify EF Core tools installed: `dotnet tool install --global dotnet-ef`
- **HTTPS certificate warnings**: 
	- Trust dev certificate: `dotnet dev-certs https --trust`
	- Or use HTTP endpoints (port 5000/7000)
- **Test failures in CI**: 
	- Check PostgreSQL service is configured in workflow
	- Verify connection string uses correct credentials

## Recent Changes

- Added OpenAPI contract and plan artifacts for backend
- UI tasks tracked in `specs/2-fancy-ui/tasks.md` (strict checklist format)
- Enhanced Copilot instructions with code quality, security, CI/CD, and troubleshooting guidelines
