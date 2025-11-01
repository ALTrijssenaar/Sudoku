using Microsoft.EntityFrameworkCore;

namespace Sudoku.Models;

/// <summary>
/// Database context for the Sudoku application.
/// </summary>
public class SudokuDbContext : DbContext
{
    public SudokuDbContext(DbContextOptions<SudokuDbContext> options)
        : base(options)
    {
    }

    public DbSet<SudokuPuzzle> Puzzles { get; set; } = null!;
    public DbSet<GameSession> Sessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure SudokuPuzzle entity
        modelBuilder.Entity<SudokuPuzzle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InitialBoard)
                .HasConversion(
                    v => SerializeBoard(v),
                    v => DeserializeBoard(v));
            entity.Property(e => e.Solution)
                .HasConversion(
                    v => SerializeBoard(v),
                    v => DeserializeBoard(v));
            entity.Property(e => e.Difficulty).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Configure GameSession entity
        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CurrentBoard)
                .HasConversion(
                    v => SerializeBoard(v),
                    v => DeserializeBoard(v));
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.StartedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
        });
    }

    private static string SerializeBoard(int[,] board)
    {
        var rows = new List<string>();
        for (int i = 0; i < 9; i++)
        {
            var row = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                row.Add(board[i, j]);
            }
            rows.Add(string.Join(",", row));
        }
        return string.Join(";", rows);
    }

    private static int[,] DeserializeBoard(string boardString)
    {
        var board = new int[9, 9];
        var rows = boardString.Split(';');
        for (int i = 0; i < 9; i++)
        {
            var cells = rows[i].Split(',');
            for (int j = 0; j < 9; j++)
            {
                board[i, j] = int.Parse(cells[j]);
            }
        }
        return board;
    }
}
