# Sudoku UI & API

A web-based Sudoku game built with .NET 9, Blazor, and PostgreSQL.

## Quick Start

### Prerequisites

- .NET 9 SDK
- PostgreSQL 16 (or use Docker)
- Visual Studio Code (recommended)

### Running with F5 (VS Code)

1. Open the project in VS Code
2. Press F5 and select "Launch Full Stack"
3. The API will start on http://localhost:5000
4. The UI will start on http://localhost:5001

### Running Manually

1. Start PostgreSQL (or use the devcontainer)
2. Run the API:
   ```bash
   cd backend/src/Sudoku.Api
   dotnet run
   ```
3. In a new terminal, run the UI:
   ```bash
   cd frontend/src/Sudoku.UI
   dotnet run
   ```

### Running with Docker (Devcontainer)

1. Open the project in VS Code
2. Click "Reopen in Container" when prompted
3. Press F5 to run

## Project Structure

```
.
├── backend/
│   ├── src/
│   │   ├── Sudoku.Api/       # REST API
│   │   ├── Sudoku.Models/    # Domain models and DB context
│   │   └── Sudoku.Services/  # Business logic
│   └── tests/
│       └── Sudoku.Tests/     # Unit tests
├── frontend/
│   ├── src/
│   │   └── Sudoku.UI/        # Blazor web app
│   └── tests/                # Playwright tests
└── docs/                     # Documentation

```

## Features

- **Generate Puzzles**: Create new Sudoku puzzles with Easy, Medium, or Hard difficulty
- **Interactive Board**: Click on cells to enter numbers
- **Move Validation**: Real-time validation of moves
- **Solution Checking**: Verify if your solution is correct
- **Persistent Storage**: Puzzles are stored in PostgreSQL

## API Endpoints

- `POST /api/puzzle/generate` - Generate a new puzzle
- `GET /api/puzzle/{id}` - Get a puzzle by ID
- `POST /api/puzzle/{id}/validate-move` - Validate a single move
- `POST /api/puzzle/{id}/validate-solution` - Check if the puzzle is solved

## Testing

### Backend Tests (xUnit)

```bash
dotnet test backend/tests/Sudoku.Tests/Sudoku.Tests.csproj
```

### Frontend Tests (Playwright)

```bash
cd frontend/tests
npm install
npm test
```

## Development

### Database Connection

The application uses PostgreSQL. The default connection string is:
```
Host=localhost;Port=5432;Database=sudoku_db;Username=sudoku;Password=sudoku_dev_password
```

Override in `appsettings.Development.json` if needed.

### Architecture

This project follows clean architecture principles:

- **Models**: Domain entities and database context
- **Services**: Business logic (puzzle generation, validation)
- **API**: RESTful endpoints
- **UI**: Blazor Server components

All business logic is in the backend. The UI is kept simple and delegates to the API.

## License

MIT
