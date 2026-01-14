using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Domain.Entities;
using VideoGameCatalogue.Domain.Interfaces;
using VideoGameCatalogue.Infrastructure.Data;

namespace VideoGameCatalogue.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for VideoGame-specific data access operations.
/// </summary>
public class VideoGameRepository : Repository<VideoGame>, IVideoGameRepository
{
    public VideoGameRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VideoGame>> GetByGenreAsync(
        string genre,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(v => v.Genre.ToLower().Contains(genre.ToLower()))
            .OrderBy(v => v.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VideoGame>> GetByPlatformAsync(
        string platform,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(v => v.Platform.ToLower().Contains(platform.ToLower()))
            .OrderBy(v => v.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VideoGame>> GetByReleaseYearAsync(
        int year,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(v => v.ReleaseYear == year)
            .OrderBy(v => v.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VideoGame>> SearchByTitleAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(v => v.Title.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(v => v.Title)
            .ToListAsync(cancellationToken);
    }
}
