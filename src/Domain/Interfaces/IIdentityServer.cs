namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for retrieving identity server configuration settings.
/// This interface is used to retrieve various identity server-related configuration values.
/// </summary>
public interface IIdentityServer
{
    /// <summary>
    /// Gets the authority URL of the identity server.
    /// </summary>
    /// <returns>The identity server authority URL.</returns>
    string GetIdentityServerAuthority();

    /// <summary>
    /// Gets the client ID used to authenticate the identity server client.
    /// </summary>
    /// <returns>The identity server client ID.</returns>
    string GetIdentityServerClientId();

    /// <summary>
    /// Gets the name associated with the identity server client.
    /// </summary>
    /// <returns>The identity server client name.</returns>
    string GetIdentityServerClientName();

    /// <summary>
    /// Gets the secret key used to authenticate the identity server client.
    /// </summary>
    /// <returns>The identity server client secret.</returns>
    string GetIdentityServerClientSecret();

    /// <summary>
    /// Gets the client ID for the interactive identity server client.
    /// </summary>
    /// <returns>The interactive identity server client ID.</returns>
    string GetIdentityServerInteractiveClientId();

    /// <summary>
    /// Gets the name associated with the interactive identity server client.
    /// </summary>
    /// <returns>The interactive identity server client name.</returns>
    string GetIdentityServerInteractiveClientName();

    /// <summary>
    /// Gets the secret key for the interactive identity server client.
    /// </summary>
    /// <returns>The interactive identity server client secret.</returns>
    string GetIdentityServerInteractiveClientSecret();

    /// <summary>
    /// Gets the scopes that are available for the identity server client.
    /// </summary>
    /// <returns>A comma-separated list of identity server scopes.</returns>
    string GetIdentityServerScopes();
}