namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for OAuth-related configurations, specifically for Google OAuth.
/// This interface provides methods to retrieve client credentials required for authenticating with Google OAuth services.
/// </summary>
public interface IOAuth
{
    /// <summary>
    /// Gets the client ID required for authenticating with Google OAuth.
    /// This client ID is used to identify the application to Google's OAuth servers during the authentication process.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the Google OAuth client ID.</returns>
    string GetGoogleClientId();

    /// <summary>
    /// Gets the client secret required for authenticating with Google OAuth.
    /// This client secret is used along with the client ID to authenticate the application securely with Google's OAuth servers.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the Google OAuth client secret.</returns>
    string GetGoogleClientSecret();
}