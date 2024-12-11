namespace Shared.SeedWork.Interfaces;

/// <summary>
/// Represents a configuration interface for OAuth settings, specifically for Google authentication.
/// This interface defines methods to retrieve the client ID and client secret for OAuth authentication with Google.
/// </summary>
public interface IOAuth
{
    /// <summary>
    /// Gets the Google OAuth client ID.
    /// This method returns the client ID used for authenticating with Google services.
    /// It may return null if the client ID is not configured.
    /// </summary>
    /// <returns>The Google OAuth client ID or null if not configured.</returns>
    string? GetGoogleClientId();

    /// <summary>
    /// Gets the Google OAuth client secret.
    /// This method returns the client secret used for authenticating with Google services.
    /// It may return null if the client secret is not configured.
    /// </summary>
    /// <returns>The Google OAuth client secret or null if not configured.</returns>
    string? GetGoogleClientSecret();
}