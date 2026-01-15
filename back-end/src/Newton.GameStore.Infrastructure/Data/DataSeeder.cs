using Newton.GameStore.Domain.Entities;

namespace Newton.GameStore.Infrastructure.Data;

/// <summary>
/// Provides seed data for the database.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.VideoGames.Any())
            return;

        var videoGames = new List<VideoGame>
        {
            new VideoGame(
                "The Legend of Zelda: Tears of the Kingdom",
                "Action-Adventure",
                "Nintendo Switch",
                2023,
                69.99m,
                "An epic adventure awaits in this sequel to Breath of the Wild.",
                "https://placehold.co/400x300/1a472a/ffffff?text=Zelda+TOTK"
            ),
            new VideoGame(
                "God of War Ragnarök",
                "Action-Adventure",
                "PlayStation 5",
                2022,
                59.99m,
                "Kratos and Atreus embark on an epic journey through the Nine Realms.",
                "https://placehold.co/400x300/2d3436/ffffff?text=God+of+War"
            ),
            new VideoGame(
                "Elden Ring",
                "Action RPG",
                "Multi-platform",
                2022,
                59.99m,
                "A dark fantasy action RPG created by FromSoftware and George R.R. Martin.",
                "https://placehold.co/400x300/4a4a4a/ffffff?text=Elden+Ring"
            ),
            new VideoGame(
                "Hogwarts Legacy",
                "Action RPG",
                "Multi-platform",
                2023,
                59.99m,
                "Experience the wizarding world in this open-world action RPG.",
                "https://placehold.co/400x300/5d4e37/ffffff?text=Hogwarts"
            ),
            new VideoGame(
                "Spider-Man 2",
                "Action-Adventure",
                "PlayStation 5",
                2023,
                69.99m,
                "Swing through New York as both Peter Parker and Miles Morales.",
                "https://placehold.co/400x300/c0392b/ffffff?text=Spider-Man+2"
            ),
            new VideoGame(
                "Starfield",
                "Action RPG",
                "Xbox/PC",
                2023,
                69.99m,
                "Explore the vastness of space in Bethesda's new sci-fi RPG.",
                "https://placehold.co/400x300/1e3799/ffffff?text=Starfield"
            )
        };

        await context.VideoGames.AddRangeAsync(videoGames);
        await context.SaveChangesAsync();
    }
}
