# GitHub Copilot Instructions (auto-generated)

**Last updated**: 2025-10-31

## Active Technologies

- .NET 8 + ASP.NET Core Web API (feature branch: 1-sudoku-platform)
- EF Core + Npgsql for PostgreSQL
- Swagger / OpenAPI (Swashbuckle)

## Goals

- Implement a Sudoku puzzle API that supports user registration/login, puzzle generation, session management (start/save/resume/complete), and user history.

## Quick commands

- Build and test: dotnet build; dotnet test
- Run locally: dotnet run (from `src/Sudoku.Api`)

## Notes

- Use JSONB for storing board states (81-element arrays). Add GIN indices if querying into boards.
- JWT bearer authentication is used for API endpoints; protect session endpoints.
- OpenAPI schema stored at `specs/1-sudoku-platform/contracts/openapi.yaml`.

## Recent changes

- 1-sudoku-platform: Added plan artifacts and OpenAPI contract.
