using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Domain.SeedWork.Enums.UserLoginData;
using Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

/// <summary>
/// Represents the login data associated with a user account.
/// </summary>
public class UserLoginDataEntity : IdentityUser<int>
{
    /// <summary>
    /// Gets or sets the universally unique identifier (Ulid) for the user.
    /// </summary>
    [Required]
    public string Uuid { get; set; } = Ulid.NewUlid().ToString();

    /// <summary>
    /// Gets or sets the identifier of the associated user account.
    /// </summary>
    // [ForeignKey("UserAccount")]
    // [Column("user_account_id")]
    [Required]
    public required int UserAccountId { get; set; }

    /// <summary>
    /// Gets or sets the hashed password for the user.
    /// </summary>
    // [Column("password_hash", TypeName = "varchar(250)")]
    // public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    // [Column("email", TypeName = "varchar(255)")]
    // public string Email { get; set; } = "";

    /// <summary>
    /// Gets or sets the status of the user's email verification.
    /// </summary>
    // [Column("email_status", TypeName = "varchar(50)")]
    public EmailStatus? EmailStatus { get; set; }

    /// <summary>
    /// Gets or sets the token used for password recovery.
    /// </summary>
    // [Column("password_recovery_token", TypeName = "varchar(1024)")]
    public string PasswordRecoveryToken { get; set; } = "";

    /// <summary>
    /// Gets or sets the token used for email confirmation.
    /// </summary>
    // [Column("confirmation_token", TypeName = "varchar(1024)")]
    public string? ConfirmationToken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
    /// </summary>
    // [Column("is_two_factor_enabled")]
    public bool IsTwoFactoeEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has verified two-factor authentication.
    /// </summary>
    // [Column("is_two_factor_verified")]
    public bool IsTwoFactorVerified { get; set; }

    /// <summary>
    /// Gets or sets the secret used for two-factor authentication.
    /// </summary>
    // [Column("two_factor_secret", TypeName = "varchar(255)")]
    public string TwoFactorSecret { get; set; } = "";

    /// <summary>
    /// Gets or sets the position of the user within the organization.
    /// </summary>
    // [Column("user_position", TypeName = "varchar(50)")]
    // public UserPosition? UserPosition { get; set; }
    
    //public BaseEntity? baseEntity { get; set; }
    
    [ForeignKey("UserAccountId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual UserAccountAggregate UserAccount { get; set; } = null!;
}
