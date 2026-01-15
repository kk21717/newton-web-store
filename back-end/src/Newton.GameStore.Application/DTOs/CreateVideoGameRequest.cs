using System.ComponentModel.DataAnnotations;

namespace Newton.GameStore.Application.DTOs;

/// <summary>
/// Request object for creating a new video game.
/// </summary>
public record CreateVideoGameRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Genre { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Platform { get; init; } = string.Empty;

    [Range(1958, 2100)]
    public int ReleaseYear { get; init; }

    [Range(0, 1000)]
    public decimal Price { get; init; }

    [StringLength(2000)]
    public string Description { get; init; } = string.Empty;

    [StringLength(500)]
    public string ImageUrl { get; init; } = string.Empty;
}
