using Domain.Entities;
using Domain.SeedWork.Enums.UserLoginData;

namespace Infrastructure.Entities;

/// <summary>
/// Represents the login data associated with a user account.
/// </summary>
public class UserLoginDataEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the user login data.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the universally unique identifier (UUID) for the user.
    /// </summary>
    public string Uuid { get; set; } = "";

    /// <summary>
    /// Gets or sets the identifier of the associated user account.
    /// </summary>
    public int UserAccountId { get; set; }

    /// <summary>
    /// Gets or sets the hashed password for the user.
    /// </summary>
    public string PasswordHash { get; set; } = "";

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Gets or sets the status of the user's email verification.
    /// </summary>
    public EmailStatus? EmailStatus { get; set; }

    /// <summary>
    /// Gets or sets the token used for password recovery.
    /// </summary>
    public string PasswordRecoveryToken { get; set; } = "";

    /// <summary>
    /// Gets or sets the token used for email confirmation.
    /// </summary>
    public string ConfirmationToken { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
    /// </summary>
    public bool IsTwoFactoeEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has verified two-factor authentication.
    /// </summary>
    public bool IsTwoFactorVerified { get; set; }

    /// <summary>
    /// Gets or sets the secret used for two-factor authentication.
    /// </summary>
    public string TwoFactorSecret { get; set; } = "";

    /// <summary>
    /// Gets or sets the position of the user within the organization.
    /// </summary>
    public UserPosition? UserPosition { get; set; }
    
    //public BaseEntity? baseEntity { get; set; }
    public UserAccountEntity UserAccount { get; set; }
}
