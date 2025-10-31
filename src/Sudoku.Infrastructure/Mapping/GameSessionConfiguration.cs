using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sudoku.Core.Entities;

namespace Sudoku.Infrastructure.Mapping;

public class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable("game_sessions");

        builder.HasKey(gs => gs.Id);

        builder.Property(gs => gs.Id)
            .HasColumnName("id");

        builder.Property(gs => gs.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasIndex(gs => gs.UserId);

        builder.Property(gs => gs.PuzzleId)
            .HasColumnName("puzzle_id")
            .IsRequired();

        builder.Property(gs => gs.CurrentState)
            .HasColumnName("current_state")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(gs => gs.StartedAt)
            .HasColumnName("started_at")
            .IsRequired();

        builder.Property(gs => gs.LastSavedAt)
            .HasColumnName("last_saved_at")
            .IsRequired();

        builder.Property(gs => gs.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(gs => gs.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(gs => gs.User)
            .WithMany(u => u.GameSessions)
            .HasForeignKey(gs => gs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gs => gs.Puzzle)
            .WithMany(p => p.GameSessions)
            .HasForeignKey(gs => gs.PuzzleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
