using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sudoku.Core.Entities;

namespace Sudoku.Infrastructure.Mapping;

public class SudokuPuzzleConfiguration : IEntityTypeConfiguration<SudokuPuzzle>
{
    public void Configure(EntityTypeBuilder<SudokuPuzzle> builder)
    {
        builder.ToTable("sudoku_puzzles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Difficulty)
            .HasColumnName("difficulty")
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.Difficulty);

        builder.Property(p => p.InitialCells)
            .HasColumnName("initial_cells")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(p => p.Solution)
            .HasColumnName("solution")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(p => p.GeneratedAt)
            .HasColumnName("generated_at")
            .IsRequired();

        builder.HasMany(p => p.GameSessions)
            .WithOne(gs => gs.Puzzle)
            .HasForeignKey(gs => gs.PuzzleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
