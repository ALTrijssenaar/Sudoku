# DevContainer Setup Guide

This project includes a complete DevContainer configuration for Visual Studio Code, providing a consistent development environment with all necessary tools pre-installed.

## What's Included

The devcontainer provides:

- **.NET 9 SDK** - Pre-installed and configured
- **PostgreSQL 16** - Running in a separate container
- **Docker-in-Docker** - For building and running containers
- **VS Code Extensions**:
  - C# Dev Kit
  - Docker extension
  - .NET Test Explorer
  - GitLens
  - EditorConfig
  - PowerShell

## Quick Start

1. **Prerequisites**:
   - Visual Studio Code
   - Docker Desktop
   - Dev Containers extension for VS Code

2. **Open in DevContainer**:
   ```bash
   # Clone the repository
   git clone https://github.com/ALTrijssenaar/Sudoku.git
   cd Sudoku
   
   # Open in VS Code
   code .
   ```

3. **Reopen in Container**:
   - VS Code will detect the devcontainer configuration
   - Click "Reopen in Container" when prompted
   - Or use Command Palette (F1): "Dev Containers: Reopen in Container"

4. **Wait for Setup**:
   - The container will build (first time only)
   - Dependencies will be restored automatically
   - Solution will be built automatically

5. **Apply Database Migrations**:
   ```bash
   cd src
   dotnet ef database update --project Sudoku.Infrastructure --startup-project Sudoku.Api
   ```

6. **Run the Application**:
   ```bash
   cd src/Sudoku.Api
   dotnet run
   ```

7. **Access the API**:
   - Swagger UI: https://localhost:5001/swagger
   - HTTP endpoint: http://localhost:5000

## Features

### Automatic Port Forwarding

The devcontainer automatically forwards these ports:
- **5000** - HTTP API endpoint
- **5001** - HTTPS API endpoint
- **5432** - PostgreSQL database (for database tools)

### Environment Variables

The devcontainer sets up these environment variables automatically:
- `ASPNETCORE_ENVIRONMENT=Development`
- `DB_HOST=localhost`
- `DB_PORT=5432`
- `DB_NAME=sudoku`
- `DB_USER=sudoku_user`
- `DB_PASSWORD=sudoku_password`
- JWT configuration for development

### Shared Network

The devcontainer shares the network with the PostgreSQL container, so you can connect to the database using `localhost:5432`.

## Troubleshooting

### Container won't start

1. Check Docker is running:
   ```bash
   docker ps
   ```

2. Rebuild the container:
   - Command Palette (F1): "Dev Containers: Rebuild Container"

### Can't connect to PostgreSQL

1. Verify PostgreSQL is running:
   ```bash
   docker compose ps
   ```

2. Check PostgreSQL logs:
   ```bash
   docker compose logs postgres
   ```

3. Restart PostgreSQL:
   ```bash
   docker compose restart postgres
   ```

### NuGet packages won't restore

1. Clear NuGet cache:
   ```bash
   dotnet nuget locals all --clear
   ```

2. Restore packages:
   ```bash
   cd src
   dotnet restore
   ```

## Benefits

Using the devcontainer ensures:

✅ **Consistency** - Everyone uses the same development environment  
✅ **Quick Setup** - New developers can start coding in minutes  
✅ **Isolation** - No conflicts with host system tools  
✅ **Reproducibility** - Same configuration across all machines  
✅ **Pre-configured** - All tools and extensions ready to use  

## Learn More

- [VS Code Dev Containers Documentation](https://code.visualstudio.com/docs/devcontainers/containers)
- [DevContainer Specification](https://containers.dev/)
