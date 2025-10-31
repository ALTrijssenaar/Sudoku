# Quickstart: Fancy, Efficient .NET Web UI (Feature 2)

## Prerequisites
- .NET 8 SDK or later
- Node.js (for frontend build tools, if needed)
- Modern browser (Chrome, Edge, Firefox, Safari)

## Setup
1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet restore` to install dependencies
4. Run `dotnet build` to build the solution
5. Run `dotnet run --project src/Sudoku.Api` to start the backend API
6. Run the UI project (e.g., `dotnet run --project src/Sudoku.UI`)
7. Open browser at `http://localhost:5000` or `https://localhost:5001`

## Usage
- Register or login as a user
- Select puzzle difficulty and start a new game
- Interact with the puzzle board
- Save progress and resume later
- View history of completed puzzles

## Developer Notes
- UI is built with Blazor WebAssembly
- API endpoints documented in `contracts/openapi.yaml`
- Accessibility and responsiveness are required
- See `data-model.md` for entities

---
