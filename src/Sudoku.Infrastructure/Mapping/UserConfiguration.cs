using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sudoku.Core.Entities;

namespace Sudoku.Infrastructure.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.DisplayName)
            .HasColumnName("display_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasMany(u => u.GameSessions)
            .WithOne(gs => gs.User)
            .HasForeignKey(gs => gs.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
