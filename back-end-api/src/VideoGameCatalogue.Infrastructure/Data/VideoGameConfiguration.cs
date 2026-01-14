using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoGameCatalogue.Domain.Entities;

namespace VideoGameCatalogue.Infrastructure.Data;

/// <summary>
/// Entity Framework configuration for the VideoGame entity.
/// </summary>
public class VideoGameConfiguration : IEntityTypeConfiguration<VideoGame>
{
    public void Configure(EntityTypeBuilder<VideoGame> builder)
    {
        builder.ToTable("VideoGames");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Platform)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.ReleaseYear)
            .IsRequired();

        builder.Property(v => v.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(v => v.Description)
            .HasMaxLength(2000);

        builder.Property(v => v.ImageUrl)
            .HasMaxLength(500);

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        // Index for common query patterns
        builder.HasIndex(v => v.Genre);
        builder.HasIndex(v => v.Platform);
        builder.HasIndex(v => v.ReleaseYear);
        builder.HasIndex(v => v.Title);
    }
}
