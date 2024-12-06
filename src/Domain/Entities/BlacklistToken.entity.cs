namespace Domain.Entities;

/// <summary>
/// Represents a blacklist token entity used for managing invalid or revoked authentication tokens.
/// This class is used to store tokens that are no longer valid or have been revoked to prevent unauthorized access.
/// </summary>
public class BlacklistTokenEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the blacklist token entity.
    /// This is the primary key for the blacklist token entity.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user account associated with the blacklisted token.
    /// This establishes a relationship between the token and the user account that the token belongs to.
    /// </summary>
    public int UserAccountId { get; set; }
    
    /// <summary>
    /// Gets or sets the token string that has been blacklisted.
    /// This token is no longer valid for authentication and is stored to prevent its reuse.
    /// </summary>
    public string Token { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the expiration date and time of the blacklisted token.
    /// This value represents the point at which the token becomes invalid and cannot be used for authentication.
    /// </summary>
    public DateTime ExpiredAt { get; set; }
    // public UserAccountEntity? User { get; set; }
}