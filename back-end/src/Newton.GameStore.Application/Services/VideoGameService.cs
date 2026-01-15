using Newton.GameStore.Application.DTOs;
using Newton.GameStore.Application.Interfaces;
using Newton.GameStore.Application.Mapping;
using Newton.GameStore.Domain.Exceptions;
using Newton.GameStore.Domain.Interfaces;

namespace Newton.GameStore.Application.Services;

/// <summary>
/// Service implementation for video game business operations.
/// Acts as the application layer orchestrator between API and Domain.
/// </summary>
public class VideoGameService : IVideoGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public VideoGameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<VideoGameDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.VideoGames.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : VideoGameMapper.ToDto(entity);
    }

    public async Task<IEnumerable<VideoGameDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.VideoGames.GetAllAsync(cancellationToken);
        return VideoGameMapper.ToDtoList(entities);
    }

    public async Task<IEnumerable<VideoGameDto>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new DomainValidationException("Genre cannot be empty.");

        var entities = await _unitOfWork.VideoGames.GetByGenreAsync(genre, cancellationToken);
        return VideoGameMapper.ToDtoList(entities);
    }

    public async Task<IEnumerable<VideoGameDto>> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new DomainValidationException("Platform cannot be empty.");

        var entities = await _unitOfWork.VideoGames.GetByPlatformAsync(platform, cancellationToken);
        return VideoGameMapper.ToDtoList(entities);
    }

    public async Task<IEnumerable<VideoGameDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var entities = await _unitOfWork.VideoGames.SearchByTitleAsync(searchTerm, cancellationToken);
        return VideoGameMapper.ToDtoList(entities);
    }

    public async Task<VideoGameDto> CreateAsync(CreateVideoGameRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = VideoGameMapper.ToEntity(request);
        await _unitOfWork.VideoGames.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return VideoGameMapper.ToDto(entity);
    }

    public async Task<VideoGameDto> UpdateAsync(int id, UpdateVideoGameRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _unitOfWork.VideoGames.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException(nameof(Domain.Entities.VideoGame), id);

        entity.Update(
            request.Title,
            request.Genre,
            request.Platform,
            request.ReleaseYear,
            request.Price,
            request.Description,
            request.ImageUrl
        );

        await _unitOfWork.VideoGames.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return VideoGameMapper.ToDto(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.VideoGames.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException(nameof(Domain.Entities.VideoGame), id);

        await _unitOfWork.VideoGames.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
