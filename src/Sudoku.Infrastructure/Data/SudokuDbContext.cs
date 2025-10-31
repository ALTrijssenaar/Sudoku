using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Entities;

namespace Sudoku.Infrastructure.Data;

public class SudokuDbContext : DbContext
{
    public SudokuDbContext(DbContextOptions<SudokuDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<SudokuPuzzle> SudokuPuzzles => Set<SudokuPuzzle>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SudokuDbContext).Assembly);
    }
}
