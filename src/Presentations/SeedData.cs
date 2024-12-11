using Persistance.Data;

namespace Presentations;

/// <summary>
/// Provides methods for seeding the initial data into the application database.
/// </summary>
public class SeedData
{
    /// <summary>
    /// Ensures that the seed data is populated in the application database.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to access the application's service scope.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
            await context.SeedAsync();
        }
    }
}