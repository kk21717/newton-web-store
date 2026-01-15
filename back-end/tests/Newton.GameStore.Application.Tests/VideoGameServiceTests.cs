using Moq;
using Newton.GameStore.Application.DTOs;
using Newton.GameStore.Application.Services;
using Newton.GameStore.Domain.Entities;
using Newton.GameStore.Domain.Exceptions;
using Newton.GameStore.Domain.Interfaces;

namespace Newton.GameStore.Application.Tests;

public class VideoGameServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly VideoGameService _service;

    public VideoGameServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IVideoGameRepository>();
        _unitOfWorkMock.Setup(u => u.VideoGames).Returns(_repositoryMock.Object);
        _service = new VideoGameService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameExists_ReturnsDto()
    {
        // Arrange
        var game = CreateTestVideoGame();
        _repositoryMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(game);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(game.Title, result.Title);
        Assert.Equal(game.Genre, result.Genre);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameDoesNotExist_ReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, default))
            .ReturnsAsync((VideoGame?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGames()
    {
        // Arrange
        var games = new List<VideoGame>
        {
            CreateTestVideoGame("Game 1"),
            CreateTestVideoGame("Game 2"),
            CreateTestVideoGame("Game 3")
        };
        _repositoryMock.Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(games);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesAndReturnsGame()
    {
        // Arrange
        var request = new CreateVideoGameRequest
        {
            Title = "New Game",
            Genre = "Action",
            Platform = "PC",
            ReleaseYear = 2023,
            Price = 59.99m,
            Description = "A new game",
            ImageUrl = "https://example.com/image.jpg"
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<VideoGame>(), default))
            .ReturnsAsync((VideoGame g, CancellationToken _) => g);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Genre, result.Genre);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenGameExists_UpdatesAndReturnsGame()
    {
        // Arrange
        var existingGame = CreateTestVideoGame();
        var request = new UpdateVideoGameRequest
        {
            Title = "Updated Title",
            Genre = "RPG",
            Platform = "PlayStation",
            ReleaseYear = 2024,
            Price = 69.99m,
            Description = "Updated description",
            ImageUrl = "https://new.example.com/image.jpg"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(existingGame);

        // Act
        var result = await _service.UpdateAsync(1, request);

        // Assert
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Genre, result.Genre);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenGameDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var request = new UpdateVideoGameRequest
        {
            Title = "Updated Title",
            Genre = "RPG",
            Platform = "PlayStation",
            ReleaseYear = 2024,
            Price = 69.99m,
            Description = "Updated description",
            ImageUrl = "https://new.example.com/image.jpg"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(999, default))
            .ReturnsAsync((VideoGame?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(999, request));
    }

    [Fact]
    public async Task DeleteAsync_WhenGameExists_DeletesGame()
    {
        // Arrange
        var existingGame = CreateTestVideoGame();
        _repositoryMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(existingGame);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(existingGame, default), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenGameDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, default))
            .ReturnsAsync((VideoGame?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.DeleteAsync(999));
    }

    [Fact]
    public async Task GetByGenreAsync_WithValidGenre_ReturnsGames()
    {
        // Arrange
        var games = new List<VideoGame>
        {
            CreateTestVideoGame("Action Game 1", "Action"),
            CreateTestVideoGame("Action Game 2", "Action")
        };
        _repositoryMock.Setup(r => r.GetByGenreAsync("Action", default))
            .ReturnsAsync(games);

        // Act
        var result = await _service.GetByGenreAsync("Action");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByGenreAsync_WithEmptyGenre_ThrowsValidationException(string? genre)
    {
        // Act & Assert
        await Assert.ThrowsAsync<DomainValidationException>(() =>
            _service.GetByGenreAsync(genre!));
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ReturnsAllGames()
    {
        // Arrange
        var games = new List<VideoGame>
        {
            CreateTestVideoGame("Game 1"),
            CreateTestVideoGame("Game 2")
        };
        _repositoryMock.Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(games);

        // Act
        var result = await _service.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void Constructor_WithNullUnitOfWork_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new VideoGameService(null!));
    }

    private static VideoGame CreateTestVideoGame(string title = "Test Game", string genre = "Action")
    {
        return new VideoGame(
            title,
            genre,
            "PC",
            2023,
            59.99m,
            "Test description",
            "https://example.com/image.jpg");
    }
}
