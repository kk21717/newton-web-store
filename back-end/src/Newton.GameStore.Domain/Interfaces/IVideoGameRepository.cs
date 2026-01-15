using Newton.GameStore.Domain.Entities;

namespace Newton.GameStore.Domain.Interfaces;

/// <summary>
/// Repository interface for VideoGame-specific data access operations.
/// </summary>
public interface IVideoGameRepository : IRepository<VideoGame>
{
    Task<IEnumerable<VideoGame>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> GetByReleaseYearAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> SearchByTitleAsync(string searchTerm, CancellationToken cancellationToken = default);
}
