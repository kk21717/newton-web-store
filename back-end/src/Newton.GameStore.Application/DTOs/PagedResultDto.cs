namespace Newton.GameStore.Application.DTOs;

/// <summary>
/// Data transfer object for paginated results.
/// </summary>
/// <typeparam name="T">The type of items in the result set.</typeparam>
public record PagedResultDto<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}
