using Newton.GameStore.Domain.Common;
using Newton.GameStore.Domain.Entities;

namespace Newton.GameStore.Domain.Interfaces;

/// <summary>
/// Repository interface for VideoGame-specific data access operations.
/// </summary>
public interface IVideoGameRepository : IRepository<VideoGame>
{
    Task<IEnumerable<VideoGame>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
    Task<PagedResult<VideoGame>> GetByGenrePagedAsync(string genre, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default);
    Task<PagedResult<VideoGame>> GetByPlatformPagedAsync(string platform, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> GetByReleaseYearAsync(int year, CancellationToken cancellationToken = default);
    Task<PagedResult<VideoGame>> GetByReleaseYearPagedAsync(int year, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<VideoGame>> SearchByTitleAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<PagedResult<VideoGame>> SearchByTitlePagedAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
