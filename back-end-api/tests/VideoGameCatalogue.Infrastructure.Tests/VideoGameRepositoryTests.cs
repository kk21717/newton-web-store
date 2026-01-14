using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Domain.Entities;
using VideoGameCatalogue.Infrastructure.Data;
using VideoGameCatalogue.Infrastructure.Repositories;

namespace VideoGameCatalogue.Infrastructure.Tests;

public class VideoGameRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly VideoGameRepository _repository;

    public VideoGameRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new VideoGameRepository(_context);
    }

    [Fact]
    public async Task AddAsync_AddsGameToDatabase()
    {
        // Arrange
        var game = CreateTestVideoGame("Test Game");

        // Act
        var result = await _repository.AddAsync(game);
        await _context.SaveChangesAsync();

        // Assert
        var savedGame = await _context.VideoGames.FirstOrDefaultAsync();
        Assert.NotNull(savedGame);
        Assert.Equal("Test Game", savedGame.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameExists_ReturnsGame()
    {
        // Arrange
        var game = CreateTestVideoGame("Test Game");
        await _context.VideoGames.AddAsync(game);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(game.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
        Assert.Equal("Test Game", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameDoesNotExist_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGames()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByGenreAsync_ReturnsMatchingGames()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.GetByGenreAsync("Action");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Contains("Action", g.Genre));
    }

    [Fact]
    public async Task GetByPlatformAsync_ReturnsMatchingGames()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.GetByPlatformAsync("PC");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Contains("PC", g.Platform));
    }

    [Fact]
    public async Task GetByReleaseYearAsync_ReturnsMatchingGames()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.GetByReleaseYearAsync(2023);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Equal(2023, g.ReleaseYear));
    }

    [Fact]
    public async Task SearchByTitleAsync_ReturnsMatchingGames()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.SearchByTitleAsync("RPG");

        // Assert
        Assert.Single(result);
        Assert.Contains("RPG", result.First().Title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesGameFromDatabase()
    {
        // Arrange
        var game = CreateTestVideoGame("Test Game");
        await _context.VideoGames.AddAsync(game);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(game);
        await _context.SaveChangesAsync();

        // Assert
        var deletedGame = await _context.VideoGames.FindAsync(game.Id);
        Assert.Null(deletedGame);
    }

    [Fact]
    public async Task ExistsAsync_WhenGameExists_ReturnsTrue()
    {
        // Arrange
        var game = CreateTestVideoGame("Test Game");
        await _context.VideoGames.AddAsync(game);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(game.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WhenGameDoesNotExist_ReturnsFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        // Arrange
        await SeedTestData();

        // Act
        var result = await _repository.CountAsync();

        // Assert
        Assert.Equal(3, result);
    }

    private async Task SeedTestData()
    {
        var games = new List<VideoGame>
        {
            CreateTestVideoGame("Action Game", "Action", "PC", 2023),
            CreateTestVideoGame("RPG Game", "RPG", "PlayStation", 2022),
            CreateTestVideoGame("Action Adventure", "Action-Adventure", "PC", 2023)
        };

        await _context.VideoGames.AddRangeAsync(games);
        await _context.SaveChangesAsync();
    }

    private static VideoGame CreateTestVideoGame(
        string title,
        string genre = "Action",
        string platform = "PC",
        int year = 2023)
    {
        return new VideoGame(
            title,
            genre,
            platform,
            year,
            59.99m,
            "Test description",
            "https://example.com/image.jpg");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
