# Sudoku.UI - Blazor WebAssembly Frontend

Modern, responsive web UI for the Sudoku Platform built with Blazor WebAssembly.

## Features

- User registration and authentication
- Interactive Sudoku puzzle board
- Multiple difficulty levels
- Save and resume game sessions
- View puzzle history
- Real-time validation
- Responsive design for desktop and mobile

## Tech Stack

- **.NET 9** - Framework
- **Blazor WebAssembly** - UI framework
- **C#** - Programming language

## Getting Started

### Prerequisites

- .NET 9 SDK
- Running Sudoku.Api backend (see main README.md)

### Running the UI

1. Ensure the backend API is running at `https://localhost:5001`

2. Navigate to the UI project:
   ```bash
   cd src/Sudoku.UI
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   - HTTPS: `https://localhost:7001`
   - HTTP: `http://localhost:7000`

## Configuration

Copy `.env.sample` to `.env` and update the values as needed:

```env
API_BASE_URL=https://localhost:5001
ENVIRONMENT=Development
```

## Project Structure

```
Sudoku.UI/
├── Pages/              # Razor pages and components
├── Layout/             # Layout components
├── Models/             # Client-side models
├── Services/           # Service layer (API clients)
├── Program.cs          # Application entry point
├── _Imports.razor      # Global using statements
├── App.razor           # Root component
└── wwwroot/            # Static assets (CSS, JS, images)
```

## Development

### Building

```bash
dotnet build
```

### Running in Development Mode

```bash
dotnet watch run
```

The application will automatically rebuild and refresh when you make changes.

## API Integration

The UI communicates with the backend API at the base URL configured in `.env`. All API endpoints are documented in the backend's Swagger UI at `https://localhost:5001/swagger`.

## Testing

```bash
dotnet test
```

## Contributing

See the main repository README.md for contribution guidelines.
