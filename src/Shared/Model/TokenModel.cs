namespace Shared.Model;

/// <summary>
/// Represents a model containing token information for authentication, including access and refresh tokens.
/// </summary>
public class TokenModel
{
    /// <summary>
    /// Gets or sets the type of the token (e.g., Bearer).
    /// </summary>
    public string TokenType  { get; set; }
    
    /// <summary>
    /// Gets or sets the access token used for authenticating API requests.
    /// </summary>
    public string AccessToken { get; set; } 
    
    /// <summary>
    /// Gets or sets the expiration time in seconds for the access token.
    /// </summary>
    public int AccessTokenExpires { get;  set; }
    
    /// <summary>
    /// Gets or sets the refresh token used to obtain a new access token when the original one expires.
    /// </summary>
    public string RefreshToken { get; set; } 
    
    /// <summary>
    /// Gets or sets the expiration time in seconds for the refresh token.
    /// </summary>
    public int RefreshTokenExpires { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenModel"/> class.
    /// </summary>
    /// <param name="tokenType">The type of the token (e.g., Bearer).</param>
    /// <param name="accessToken">The access token.</param>
    /// <param name="accessTokenExpires">The expiration time of the access token in seconds.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="refreshTokenExpires">The expiration time of the refresh token in seconds.</param>
    public TokenModel(string tokenType, string accessToken, int accessTokenExpires,  string refreshToken, int refreshTokenExpires)
    {
        TokenType = tokenType;
        AccessTokenExpires = accessTokenExpires;
        RefreshTokenExpires = refreshTokenExpires;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}