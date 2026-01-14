using VideoGameCatalogue.Application.DTOs;

namespace VideoGameCatalogue.Application.Interfaces;

/// <summary>
/// Service interface for video game business operations.
/// </summary>
public interface IVideoGameService
{
    Task<VideoGameDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGameDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGameDto>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGameDto>> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGameDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<VideoGameDto> CreateAsync(CreateVideoGameRequest request, CancellationToken cancellationToken = default);
    Task<VideoGameDto> UpdateAsync(int id, UpdateVideoGameRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
