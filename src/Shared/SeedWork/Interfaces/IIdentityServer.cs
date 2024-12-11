namespace Shared.SeedWork.Interfaces;

/// <summary>
/// Represents a configuration interface for IdentityServer settings.
/// This interface defines methods to retrieve various configuration values required for interacting with IdentityServer, 
/// such as authority, client IDs, client secrets, and scopes for both interactive and non-interactive clients.
/// </summary>
public interface IIdentityServer
{
    /// <summary>
    /// Gets the IdentityServer authority URL.
    /// This method returns the authority URL used to interact with the IdentityServer.
    /// It may return null if the authority is not configured.
    /// </summary>
    /// <returns>The IdentityServer authority URL or null if not configured.</returns>
    string? GetIdentityServerAuthority();

    /// <summary>
    /// Gets the IdentityServer client ID for non-interactive clients.
    /// This method returns the client ID used by a non-interactive client to authenticate with IdentityServer.
    /// It may return null if the client ID is not configured.
    /// </summary>
    /// <returns>The IdentityServer client ID for non-interactive clients or null if not configured.</returns>
    string? GetIdentityServerClientId();

    /// <summary>
    /// Gets the IdentityServer client name for non-interactive clients.
    /// This method returns the client name used for the non-interactive client.
    /// It may return null if the client name is not configured.
    /// </summary>
    /// <returns>The IdentityServer client name for non-interactive clients or null if not configured.</returns>
    string? GetIdentityServerClientName();

    /// <summary>
    /// Gets the IdentityServer client secret for non-interactive clients.
    /// This method returns the client secret used by a non-interactive client to authenticate with IdentityServer.
    /// It may return null if the client secret is not configured.
    /// </summary>
    /// <returns>The IdentityServer client secret for non-interactive clients or null if not configured.</returns>
    string? GetIdentityServerClientSecret();

    /// <summary>
    /// Gets the IdentityServer client ID for interactive clients.
    /// This method returns the client ID used by an interactive client (e.g., web apps or mobile apps) to authenticate with IdentityServer.
    /// It may return null if the client ID is not configured.
    /// </summary>
    /// <returns>The IdentityServer client ID for interactive clients or null if not configured.</returns>
    string? GetIdentityServerInteractiveClientId();

    /// <summary>
    /// Gets the IdentityServer client name for interactive clients.
    /// This method returns the client name used for the interactive client (e.g., web apps or mobile apps).
    /// It may return null if the client name is not configured.
    /// </summary>
    /// <returns>The IdentityServer client name for interactive clients or null if not configured.</returns>
    string? GetIdentityServerInteractiveClientName();

    /// <summary>
    /// Gets the IdentityServer client secret for interactive clients.
    /// This method returns the client secret used by interactive clients to authenticate with IdentityServer.
    /// It may return null if the client secret is not configured.
    /// </summary>
    /// <returns>The IdentityServer client secret for interactive clients or null if not configured.</returns>
    string? GetIdentityServerInteractiveClientSecret();

    /// <summary>
    /// Gets the IdentityServer scopes.
    /// This method returns the scopes required for access to various protected resources in IdentityServer.
    /// It may return null if the scopes are not configured.
    /// </summary>
    /// <returns>The IdentityServer scopes or null if not configured.</returns>
    string? GetIdentityServerScopes();
}