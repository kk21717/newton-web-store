using Microsoft.AspNetCore.Mvc;
using Newton.GameStore.Application.DTOs;
using Newton.GameStore.Application.Interfaces;

namespace Newton.GameStore.API.Controllers;

/// <summary>
/// API controller for managing video games in the catalogue.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VideoGamesController : ControllerBase
{
    private readonly IVideoGameService _videoGameService;
    private readonly ILogger<VideoGamesController> _logger;

    public VideoGamesController(
        IVideoGameService videoGameService,
        ILogger<VideoGamesController> logger)
    {
        _videoGameService = videoGameService ?? throw new ArgumentNullException(nameof(videoGameService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all video games in the catalogue with optional pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (1-based). If not provided, returns all results.</param>
    /// <param name="pageSize">Number of items per page. Default is 10.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VideoGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResultDto<VideoGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? pageNumber,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all video games (page: {PageNumber}, size: {PageSize})", pageNumber, pageSize);

        if (pageNumber.HasValue)
        {
            var pagedResult = await _videoGameService.GetAllPagedAsync(pageNumber.Value, pageSize, cancellationToken);
            return Ok(pagedResult);
        }

        var games = await _videoGameService.GetAllAsync(cancellationToken);
        return Ok(games);
    }

    /// <summary>
    /// Gets a specific video game by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoGameDto>> GetById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting video game with ID: {Id}", id);
        var game = await _videoGameService.GetByIdAsync(id, cancellationToken);

        if (game is null)
        {
            _logger.LogWarning("Video game with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(game);
    }

    /// <summary>
    /// Searches video games by title with optional pagination.
    /// </summary>
    /// <param name="term">Search term.</param>
    /// <param name="pageNumber">Page number (1-based). If not provided, returns all results.</param>
    /// <param name="pageSize">Number of items per page. Default is 10.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<VideoGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResultDto<VideoGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? term,
        [FromQuery] int? pageNumber,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching video games with term: {Term} (page: {PageNumber}, size: {PageSize})", term, pageNumber, pageSize);

        if (pageNumber.HasValue)
        {
            var pagedResult = await _videoGameService.SearchPagedAsync(term ?? string.Empty, pageNumber.Value, pageSize, cancellationToken);
            return Ok(pagedResult);
        }

        var games = await _videoGameService.SearchAsync(term ?? string.Empty, cancellationToken);
        return Ok(games);
    }

    /// <summary>
    /// Gets video games by genre with optional pagination.
    /// </summary>
    /// <param name="genre">Genre to filter by.</param>
    /// <param name="pageNumber">Page number (1-based). If not provided, returns all results.</param>
    /// <param name="pageSize">Number of items per page. Default is 10.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("genre/{genre}")]
    [ProducesResponseType(typeof(IEnumerable<VideoGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResultDto<VideoGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByGenre(
        string genre,
        [FromQuery] int? pageNumber,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting video games by genre: {Genre} (page: {PageNumber}, size: {PageSize})", genre, pageNumber, pageSize);

        if (pageNumber.HasValue)
        {
            var pagedResult = await _videoGameService.GetByGenrePagedAsync(genre, pageNumber.Value, pageSize, cancellationToken);
            return Ok(pagedResult);
        }

        var games = await _videoGameService.GetByGenreAsync(genre, cancellationToken);
        return Ok(games);
    }

    /// <summary>
    /// Gets video games by platform with optional pagination.
    /// </summary>
    /// <param name="platform">Platform to filter by.</param>
    /// <param name="pageNumber">Page number (1-based). If not provided, returns all results.</param>
    /// <param name="pageSize">Number of items per page. Default is 10.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("platform/{platform}")]
    [ProducesResponseType(typeof(IEnumerable<VideoGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResultDto<VideoGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPlatform(
        string platform,
        [FromQuery] int? pageNumber,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting video games by platform: {Platform} (page: {PageNumber}, size: {PageSize})", platform, pageNumber, pageSize);

        if (pageNumber.HasValue)
        {
            var pagedResult = await _videoGameService.GetByPlatformPagedAsync(platform, pageNumber.Value, pageSize, cancellationToken);
            return Ok(pagedResult);
        }

        var games = await _videoGameService.GetByPlatformAsync(platform, cancellationToken);
        return Ok(games);
    }

    /// <summary>
    /// Creates a new video game.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoGameDto>> Create(
        [FromBody] CreateVideoGameRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new video game: {Title}", request.Title);
        var game = await _videoGameService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
    }

    /// <summary>
    /// Updates an existing video game.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoGameDto>> Update(
        int id,
        [FromBody] UpdateVideoGameRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating video game with ID: {Id}", id);
        var game = await _videoGameService.UpdateAsync(id, request, cancellationToken);
        return Ok(game);
    }

    /// <summary>
    /// Deletes a video game.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting video game with ID: {Id}", id);
        await _videoGameService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
