using VideoGameCatalogue.Domain.Interfaces;
using VideoGameCatalogue.Infrastructure.Data;

namespace VideoGameCatalogue.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions across repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IVideoGameRepository? _videoGames;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IVideoGameRepository VideoGames =>
        _videoGames ??= new VideoGameRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
