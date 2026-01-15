namespace Newton.GameStore.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IVideoGameRepository VideoGames { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
