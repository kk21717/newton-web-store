namespace VideoGameCatalogue.Application.DTOs;

/// <summary>
/// Data transfer object for reading video game data.
/// </summary>
public record VideoGameDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Genre { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
