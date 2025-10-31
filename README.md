# Sudoku Platform

An individual user platform to solve Sudoku games built with .NET, PostgreSQL, and Docker.

## Quick Start

### 5-Minute Setup

1. **Clone and start the database:**
   ```bash
   git clone https://github.com/ALTrijssenaar/Sudoku.git
   cd Sudoku
   docker compose up -d
   ```

2. **Run the application:**
   ```bash
   cd src/Sudoku.Api
   dotnet run
   ```

3. **Access the API:**
   - Open your browser to `https://localhost:5001` or `http://localhost:5000`
   - Swagger UI with interactive docs is available at the root URL
   - Demo user: `demo@example.com` / `DemoPassword123`

### Try It Out

```bash
# Register a new user
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"you@example.com","password":"YourPassword123","displayName":"Your Name"}'

# Login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"you@example.com","password":"YourPassword123"}'

# Get a puzzle (Easy difficulty)
curl https://localhost:5001/api/puzzles?difficulty=Easy
```

## Features

- User registration and authentication (JWT)
- Multiple difficulty levels (Easy, Medium, Hard)
- Save and resume puzzle progress
- View puzzle history
- Interactive Sudoku board validation
- RESTful API with OpenAPI/Swagger documentation
- Error handling and correlation IDs

## Tech Stack

- **.NET 9** - Application framework
- **ASP.NET Core Web API** - REST API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **Docker** - Containerization
- **Swagger/OpenAPI** - API documentation
- **JWT Bearer Authentication** - Security
- **xUnit** - Testing framework

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker and Docker Compose
- (Optional) Visual Studio Code with Dev Containers extension

### Option 1: Using DevContainer (Recommended)

1. Open the project in Visual Studio Code
2. When prompted, click "Reopen in Container" (or use Command Palette: "Dev Containers: Reopen in Container")
3. The devcontainer will automatically:
   - Set up the .NET environment
   - Start PostgreSQL
   - Restore NuGet packages
   - Build the solution

4. Apply database migrations:
   ```bash
   cd src
   dotnet ef database update --project Sudoku.Infrastructure --startup-project Sudoku.Api
   ```

5. Run the API:
   ```bash
   cd src/Sudoku.Api
   dotnet run
   ```

6. Access Swagger UI at: `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

### Option 2: Local Development

1. Start PostgreSQL with Docker Compose:
   ```bash
   docker compose up -d
   ```

2. Build the solution:
   ```bash
   cd src
   dotnet build
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update --project Sudoku.Infrastructure --startup-project Sudoku.Api
   ```

4. Run the API:
   ```bash
   cd Sudoku.Api
   dotnet run
   ```

5. Access Swagger UI at: `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

## Project Structure

```
src/
├── Sudoku.sln                    # Solution file
├── Sudoku.Api/                   # Web API project
│   ├── Controllers/              # API controllers
│   ├── Program.cs               # Application entry point
│   └── appsettings.json         # Configuration
├── Sudoku.Core/                 # Domain models and interfaces
│   ├── Entities/                # Domain entities
│   ├── Repositories/            # Repository interfaces
│   └── Services/                # Service interfaces
└── Sudoku.Infrastructure/       # Data access and services
    ├── Data/                    # DbContext and seed data
    ├── Mapping/                 # EF Core configurations
    ├── Migrations/              # Database migrations
    ├── Repositories/            # Repository implementations
    └── Services/                # Service implementations
```

## Configuration

Copy `.env.sample` to `.env` and update the values:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=sudoku
DB_USER=sudoku_user
DB_PASSWORD=sudoku_password

JWT_SECRET=your-secret-key-here-change-in-production-min-32-chars
JWT_ISSUER=SudokuApi
JWT_AUDIENCE=SudokuClient
JWT_EXPIRY_MINUTES=60
```

## Database

The project uses PostgreSQL with Entity Framework Core. Migrations are located in `src/Sudoku.Infrastructure/Migrations/`.

### Common EF Core Commands

```bash
# Create a new migration
dotnet ef migrations add MigrationName --project Sudoku.Infrastructure --startup-project Sudoku.Api

# Apply migrations
dotnet ef database update --project Sudoku.Infrastructure --startup-project Sudoku.Api

# Remove last migration
dotnet ef migrations remove --project Sudoku.Infrastructure --startup-project Sudoku.Api
```

## API Documentation

Once the application is running, you can access:

- **Swagger UI**: `https://localhost:5001/swagger` (interactive API documentation)
- **OpenAPI Spec**: `specs/1-sudoku-platform/contracts/openapi.yaml`
- **Postman Collection**: `docs/postman/Sudoku-API.postman_collection.json`

### Using the Postman Collection

1. Import `docs/postman/Sudoku-API.postman_collection.json` into Postman
2. Set the `baseUrl` variable to your API endpoint (default: `http://localhost:5000`)
3. Start with the Authentication folder to register/login
4. Use returned IDs in subsequent requests

## Development

### Running Tests

```bash
cd src
dotnet test
```

### Building

```bash
cd src
dotnet build
```

### Code Style

The project follows standard .NET coding conventions. Use EditorConfig for consistent formatting.

## License

This project is licensed under the MIT License.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request
