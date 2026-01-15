using Newton.GameStore.Domain.Entities;
using Newton.GameStore.Domain.Exceptions;

namespace Newton.GameStore.Domain.Tests;

public class VideoGameTests
{
    private const string ValidTitle = "Test Game";
    private const string ValidGenre = "Action";
    private const string ValidPlatform = "PC";
    private const int ValidYear = 2023;
    private const decimal ValidPrice = 59.99m;
    private const string ValidDescription = "A test game description";
    private const string ValidImageUrl = "https://example.com/image.jpg";

    [Fact]
    public void Constructor_WithValidParameters_CreatesVideoGame()
    {
        // Act
        var game = new VideoGame(
            ValidTitle,
            ValidGenre,
            ValidPlatform,
            ValidYear,
            ValidPrice,
            ValidDescription,
            ValidImageUrl);

        // Assert
        Assert.Equal(ValidTitle, game.Title);
        Assert.Equal(ValidGenre, game.Genre);
        Assert.Equal(ValidPlatform, game.Platform);
        Assert.Equal(ValidYear, game.ReleaseYear);
        Assert.Equal(ValidPrice, game.Price);
        Assert.Equal(ValidDescription, game.Description);
        Assert.Equal(ValidImageUrl, game.ImageUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetTitle_WithEmptyOrNull_ThrowsValidationException(string? invalidTitle)
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        Assert.Throws<DomainValidationException>(() => game.SetTitle(invalidTitle!));
    }

    [Fact]
    public void SetTitle_WithTooLongTitle_ThrowsValidationException()
    {
        // Arrange
        var game = CreateValidVideoGame();
        var longTitle = new string('a', 201);

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => game.SetTitle(longTitle));
        Assert.Contains("200 characters", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetGenre_WithEmptyOrNull_ThrowsValidationException(string? invalidGenre)
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        Assert.Throws<DomainValidationException>(() => game.SetGenre(invalidGenre!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetPlatform_WithEmptyOrNull_ThrowsValidationException(string? invalidPlatform)
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        Assert.Throws<DomainValidationException>(() => game.SetPlatform(invalidPlatform!));
    }

    [Theory]
    [InlineData(1900)]
    [InlineData(2100)]
    public void SetReleaseYear_WithInvalidYear_ThrowsValidationException(int invalidYear)
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        Assert.Throws<DomainValidationException>(() => game.SetReleaseYear(invalidYear));
    }

    [Fact]
    public void SetPrice_WithNegativeValue_ThrowsValidationException()
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => game.SetPrice(-1));
        Assert.Contains("negative", exception.Message);
    }

    [Fact]
    public void SetPrice_WithExcessiveValue_ThrowsValidationException()
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => game.SetPrice(1001));
        Assert.Contains("1000", exception.Message);
    }

    [Fact]
    public void SetPrice_RoundsToTwoDecimalPlaces()
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act
        game.SetPrice(19.999m);

        // Assert
        Assert.Equal(20.00m, game.Price);
    }

    [Fact]
    public void Update_WithValidParameters_UpdatesAllProperties()
    {
        // Arrange
        var game = CreateValidVideoGame();
        var newTitle = "Updated Game";
        var newGenre = "RPG";
        var newPlatform = "PlayStation";
        var newYear = 2024;
        var newPrice = 69.99m;
        var newDescription = "Updated description";
        var newImageUrl = "https://new.example.com/image.jpg";

        // Act
        game.Update(newTitle, newGenre, newPlatform, newYear, newPrice, newDescription, newImageUrl);

        // Assert
        Assert.Equal(newTitle, game.Title);
        Assert.Equal(newGenre, game.Genre);
        Assert.Equal(newPlatform, game.Platform);
        Assert.Equal(newYear, game.ReleaseYear);
        Assert.Equal(newPrice, game.Price);
        Assert.Equal(newDescription, game.Description);
        Assert.Equal(newImageUrl, game.ImageUrl);
    }

    [Fact]
    public void SetDescription_WithNull_SetsEmptyString()
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act
        game.SetDescription(null!);

        // Assert
        Assert.Equal(string.Empty, game.Description);
    }

    [Fact]
    public void SetImageUrl_WithNull_SetsEmptyString()
    {
        // Arrange
        var game = CreateValidVideoGame();

        // Act
        game.SetImageUrl(null!);

        // Assert
        Assert.Equal(string.Empty, game.ImageUrl);
    }

    private static VideoGame CreateValidVideoGame()
    {
        return new VideoGame(
            ValidTitle,
            ValidGenre,
            ValidPlatform,
            ValidYear,
            ValidPrice,
            ValidDescription,
            ValidImageUrl);
    }
}
