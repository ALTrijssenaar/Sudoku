# quickstart.md

This quickstart gets you a local development environment for the Sudoku API using .NET and PostgreSQL.

Prerequisites

- .NET 8 SDK installed
- Docker and Docker Compose installed
- Optional: Postman or curl for API testing

Steps

1. Start Postgres with Docker Compose (from repo root):

```powershell
docker compose up -d
```

2. Build and run the API locally (run from repo root):

```powershell
cd src/Sudoku.Api
dotnet run
```

3. Apply EF Core migrations (after DbContext configured):

```powershell
dotnet ef database update -p src/Sudoku.Infrastructure -s src/Sudoku.Api
```

4. Open Swagger UI (default):

- Visit: https://localhost:5001/swagger or http://localhost:5000/swagger (depending on launch settings)

5. Use the Auth endpoints to register and login, then create a session and interact with puzzle endpoints.

Notes

- The repository contains seed data in `src/Sudoku.Infrastructure/Data/SeedPuzzles` for initial puzzles.
- To run integration tests that require Postgres, ensure docker compose is up before running the test suite.

*** End of quickstart.md
