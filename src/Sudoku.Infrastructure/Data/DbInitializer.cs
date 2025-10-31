using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sudoku.Core.Entities;
using Sudoku.Core.Security;

namespace Sudoku.Infrastructure.Data;

public class DbInitializer
{
    private readonly SudokuDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(SudokuDbContext context, IPasswordHasher passwordHasher, ILogger<DbInitializer> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();

            // Seed demo user if not exists
            await SeedDemoUserAsync();

            // Seed puzzles if not exists
            await SeedPuzzlesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedDemoUserAsync()
    {
        var demoUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        if (!await _context.Users.AnyAsync(u => u.Id == demoUserId))
        {
            var demoUser = new User
            {
                Id = demoUserId,
                Email = "demo@example.com",
                DisplayName = "Demo User",
                PasswordHash = _passwordHasher.HashPassword("DemoPassword123"),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(demoUser);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Demo user seeded: {Email}", demoUser.Email);
        }
    }

    private async Task SeedPuzzlesAsync()
    {
        if (await _context.SudokuPuzzles.AnyAsync())
        {
            return; // Puzzles already exist
        }

        var seedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedPuzzles");
        
        if (!Directory.Exists(seedPath))
        {
            _logger.LogWarning("Seed puzzles directory not found at {Path}", seedPath);
            return;
        }

        var puzzleFiles = Directory.GetFiles(seedPath, "*.json");
        
        foreach (var file in puzzleFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var puzzleData = JsonSerializer.Deserialize<SeedPuzzleData>(json);
                
                if (puzzleData != null)
                {
                    var puzzle = new SudokuPuzzle
                    {
                        Id = Guid.NewGuid(),
                        Difficulty = puzzleData.Difficulty,
                        InitialCells = puzzleData.InitialCells,
                        Solution = puzzleData.Solution,
                        GeneratedAt = DateTime.UtcNow
                    };

                    _context.SudokuPuzzles.Add(puzzle);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding puzzle from file {File}", file);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Seed puzzles loaded from {Path}", seedPath);
    }

    private class SeedPuzzleData
    {
        public string Difficulty { get; set; } = string.Empty;
        public int[] InitialCells { get; set; } = Array.Empty<int>();
        public int[] Solution { get; set; } = Array.Empty<int>();
    }
}
