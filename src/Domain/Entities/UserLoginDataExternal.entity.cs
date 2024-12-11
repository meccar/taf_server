namespace Domain.Entities;

/// <summary>
/// Represents external login data associated with a user's account.
/// This entity stores information related to an external authentication provider, such as a social login token.
/// </summary>
public class UserLoginDataExternalEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the external login data entity.
    /// This is a primary key used to uniquely identify the external login data in the system.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the universally unique identifier (ULID) for the external login data entity.
    /// This identifier is used to uniquely identify the external login data across different systems.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the social ID for the external login data.
    /// This represents the unique identifier associated with the user's login from an external provider (e.g., Facebook, Google).
    /// </summary>
    public string SsocialId { get; set; } = "";
    
    // public Provider Provider { get; set; }
    
    /// <summary>
    /// Gets or sets the token associated with the external login.
    /// This token is used for authentication or authorization via the external provider (e.g., an OAuth token).
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// Gets or sets the expiration date and time of the external login token.
    /// This property defines when the token will expire and can no longer be used for authentication.
    /// </summary>
    public DateTime EpiredAt { get; set; }
    
    // public UserAccountEntity? UserAccount { get; set; }
}