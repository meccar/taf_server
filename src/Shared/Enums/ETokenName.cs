namespace Shared.Enums;

/// <summary>
/// Represents the types of tokens used in the authentication system.
/// </summary>
public enum ETokenName
{
    /// <summary>
    /// The refresh token, used to obtain a new access token.
    /// </summary>
    REFRESH,

    /// <summary>
    /// The access token, used to authenticate API requests.
    /// </summary>
    ACCESS
}