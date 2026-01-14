using VideoGameCatalogue.Application.DTOs;
using VideoGameCatalogue.Domain.Entities;

namespace VideoGameCatalogue.Application.Mapping;

/// <summary>
/// Mapper for converting between VideoGame entities and DTOs.
/// Uses static methods for simplicity - consider AutoMapper for larger projects.
/// </summary>
public static class VideoGameMapper
{
    public static VideoGameDto ToDto(VideoGame entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VideoGameDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Genre = entity.Genre,
            Platform = entity.Platform,
            ReleaseYear = entity.ReleaseYear,
            Price = entity.Price,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static IEnumerable<VideoGameDto> ToDtoList(IEnumerable<VideoGame> entities)
    {
        return entities.Select(ToDto);
    }

    public static VideoGame ToEntity(CreateVideoGameRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new VideoGame(
            request.Title,
            request.Genre,
            request.Platform,
            request.ReleaseYear,
            request.Price,
            request.Description,
            request.ImageUrl
        );
    }
}
