# Quickstart: Fancy, Efficient .NET Web UI (Feature 2)

## Prerequisites
- .NET 9 SDK
- PostgreSQL database (for backend)
- Modern browser (Chrome, Edge, Firefox, Safari)

## Setup

### 1. Clone and Build
```bash
git clone <repository-url>
cd Sudoku
dotnet restore
dotnet build
```

### 2. Configure Backend
```bash
cd src/Sudoku.Api
# Copy .env.sample to .env and configure database connection
cp .env.sample .env
# Edit .env with your PostgreSQL connection string
```

### 3. Start Backend API
```bash
dotnet run --project src/Sudoku.Api
# API will be available at https://localhost:5001
# Swagger UI at https://localhost:5001/swagger
```

### 4. Configure Frontend
```bash
cd src/Sudoku.UI
# Configure API base URL (default is https://localhost:5001)
# Edit appsettings.json or set ApiBaseUrl in browser developer tools
```

### 5. Start Frontend UI
```bash
dotnet run --project src/Sudoku.UI
# UI will be available at:
# - HTTPS: https://localhost:7001
# - HTTP: http://localhost:7000
```

## Usage

### First Time Users
1. Navigate to https://localhost:7001
2. Click "Get Started" or "Register"
3. Create an account with email and password
4. You'll be automatically logged in and redirected to the dashboard

### Playing Puzzles
1. From the dashboard, select a difficulty level (Easy, Medium, or Hard)
2. A new puzzle will start immediately
3. Click on any empty cell to select it
4. Use the number pad to enter values (1-9)
5. Invalid moves will be rejected with a warning
6. Click "Save Progress" to save your current state
7. Click "Check Solution" when you think you've completed the puzzle

### Resume & History
- Active sessions appear on the dashboard with "Resume" buttons
- View completed puzzles in the "History" page
- See statistics like completion time and total puzzles solved

## Features

### Implemented
- ✅ User registration and login with JWT authentication
- ✅ Multiple difficulty levels (Easy, Medium, Hard)
- ✅ Interactive 9x9 Sudoku grid with visual feedback
- ✅ Real-time move validation (Sudoku rules)
- ✅ Save and resume game sessions
- ✅ Complete puzzle history with statistics
- ✅ Responsive design for mobile and desktop
- ✅ Material Design UI (MudBlazor)

### Tech Stack
- **Frontend**: Blazor WebAssembly (.NET 9)
- **UI Framework**: MudBlazor 8.13
- **Backend**: ASP.NET Core Web API
- **Database**: PostgreSQL with EF Core
- **Authentication**: JWT Bearer tokens

## Developer Notes

### Project Structure
```
src/Sudoku.UI/
├── Pages/              # Razor pages (Register, Login, Dashboard, Puzzle, History)
├── Layout/             # Layout components (MainLayout, NavMenu)
├── Models/             # Client-side models
├── Services/           # Service layer (ApiService, AuthService, BoardValidator)
├── Shared/             # Shared components
└── wwwroot/            # Static assets
```

### API Integration
- All API calls go through `ApiService.cs`
- Authentication handled by `AuthService.cs`
- JWT tokens automatically included in requests
- API base URL configured in `Program.cs` (default: https://localhost:5001)

### Validation
- Client-side Sudoku rule validation via `BoardValidator`
- Invalid moves are prevented with user feedback
- Solution validation before submission

### Accessibility
- WCAG 2.1 AA compliant via MudBlazor
- Keyboard navigation supported
- Screen reader compatible
- High contrast support

### Browser Compatibility
- Chrome 90+
- Edge 90+
- Firefox 88+
- Safari 14+

## Troubleshooting

### "Failed to load puzzle"
- Ensure backend API is running at https://localhost:5001
- Check browser console for CORS errors
- Verify database connection in backend

### Authentication Issues
- Clear browser local storage
- Check JWT secret is configured in backend appsettings.json
- Verify token expiration time (default 60 minutes)

### Build Errors
- Run `dotnet clean` then `dotnet restore`
- Ensure .NET 9 SDK is installed
- Check NuGet package restore

---
