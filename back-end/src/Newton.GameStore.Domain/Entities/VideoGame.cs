using Newton.GameStore.Domain.Common;
using Newton.GameStore.Domain.Exceptions;

namespace Newton.GameStore.Domain.Entities;

/// <summary>
/// Represents a video game in the catalogue.
/// </summary>
public class VideoGame : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Genre { get; private set; } = string.Empty;
    public string Platform { get; private set; } = string.Empty;
    public int ReleaseYear { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;

    // Required for EF Core
    private VideoGame() { }

    public VideoGame(
        string title,
        string genre,
        string platform,
        int releaseYear,
        decimal price,
        string description,
        string imageUrl)
    {
        SetTitle(title);
        SetGenre(genre);
        SetPlatform(platform);
        SetReleaseYear(releaseYear);
        SetPrice(price);
        SetDescription(description);
        SetImageUrl(imageUrl);
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Title cannot be empty.");

        if (title.Length > 200)
            throw new DomainValidationException("Title cannot exceed 200 characters.");

        Title = title.Trim();
        SetUpdatedAt();
    }

    public void SetGenre(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new DomainValidationException("Genre cannot be empty.");

        Genre = genre.Trim();
        SetUpdatedAt();
    }

    public void SetPlatform(string platform)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new DomainValidationException("Platform cannot be empty.");

        Platform = platform.Trim();
        SetUpdatedAt();
    }

    public void SetReleaseYear(int releaseYear)
    {
        const int earliestYear = 1958; // Tennis for Two - first video game
        var currentYear = DateTime.UtcNow.Year;

        if (releaseYear < earliestYear || releaseYear > currentYear + 5)
            throw new DomainValidationException($"Release year must be between {earliestYear} and {currentYear + 5}.");

        ReleaseYear = releaseYear;
        SetUpdatedAt();
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new DomainValidationException("Price cannot be negative.");

        if (price > 1000)
            throw new DomainValidationException("Price cannot exceed $1000.");

        Price = Math.Round(price, 2);
        SetUpdatedAt();
    }

    public void SetDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
        SetUpdatedAt();
    }

    public void SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl?.Trim() ?? string.Empty;
        SetUpdatedAt();
    }

    public void Update(
        string title,
        string genre,
        string platform,
        int releaseYear,
        decimal price,
        string description,
        string imageUrl)
    {
        SetTitle(title);
        SetGenre(genre);
        SetPlatform(platform);
        SetReleaseYear(releaseYear);
        SetPrice(price);
        SetDescription(description);
        SetImageUrl(imageUrl);
    }
}
