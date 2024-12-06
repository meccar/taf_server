namespace Shared.Model;

/// <summary>
/// Represents a user account with associated details.
/// </summary>
public class UserAccountModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user account.
    /// </summary>
    public string Id { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the external identifier for the user account.
    /// </summary>
    public string EId { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the unique identifier of the associated user profile.
    /// </summary>
    public string UserProfileId { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the plaintext password of the user.
    /// </summary>
    public string Password { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    public string PasswordHash { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the security stamp used for security purposes.
    /// </summary>
    public string? SecurityStamp { get; set; }
    
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the status of the email (e.g., verified, unverified).
    /// </summary>
    public string EmailStatus { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the token used for password recovery.
    /// </summary>
    public string PasswordRecoveryToken { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the token used for email confirmation.
    /// </summary>
    public string ConfirmationToken { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is enabled.
    /// </summary>
    public bool IsTwoFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication has been verified.
    /// </summary>
    public bool IsTwoFactorVerified { get; set; }
    
    /// <summary>
    /// Gets or sets the associated user profile.
    /// </summary>
    public UserProfileModel? UserAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user account was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user account was deleted.
    /// </summary>
    public DateTime DeletedAt { get; set; }
}