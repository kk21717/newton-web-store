using Microsoft.EntityFrameworkCore;
using Newton.GameStore.Domain.Common;
using Newton.GameStore.Domain.Entities;
using Newton.GameStore.Domain.Interfaces;
using Newton.GameStore.Infrastructure.Data;

namespace Newton.GameStore.Infrastructure.Repositories;

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

    public async Task<PagedResult<VideoGame>> GetByGenrePagedAsync(
        string genre,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(v => v.Genre.ToLower().Contains(genre.ToLower()))
            .OrderBy(v => v.Title);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<VideoGame>(items, totalCount, pageNumber, pageSize);
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

    public async Task<PagedResult<VideoGame>> GetByPlatformPagedAsync(
        string platform,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(v => v.Platform.ToLower().Contains(platform.ToLower()))
            .OrderBy(v => v.Title);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<VideoGame>(items, totalCount, pageNumber, pageSize);
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

    public async Task<PagedResult<VideoGame>> GetByReleaseYearPagedAsync(
        int year,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(v => v.ReleaseYear == year)
            .OrderBy(v => v.Title);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<VideoGame>(items, totalCount, pageNumber, pageSize);
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

    public async Task<PagedResult<VideoGame>> SearchByTitlePagedAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(v => v.Title.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(v => v.Title);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<VideoGame>(items, totalCount, pageNumber, pageSize);
    }
}
