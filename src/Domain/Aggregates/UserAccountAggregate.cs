using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents the login data associated with a user account.
/// </summary>
public class UserAccountAggregate : IdentityUser<int>
{
    /// <summary>
    /// Gets or sets the universally unique identifier (Ulid) for the user.
    /// </summary>
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string EId { get; set; } = Ulid.NewUlid().ToString();

    /// <summary>
    /// Gets or sets the identifier of the associated user account.
    /// </summary>
    [Required]
    public required int UserProfileId { get; set; }


    /// <summary>
    /// Gets or sets the status of the user's email verification.
    /// </summary>
    public string EmailStatus { get; set; } = EEmailStatus.Pending.ToString();

    /// <summary>
    /// Gets or sets the token used for password recovery.
    /// </summary>
    public string PasswordRecoveryToken { get; set; } = null;

    /// <summary>
    /// Gets or sets the token used for email confirmation.
    /// </summary>
    public string? ConfirmationToken { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
    /// </summary>
    public bool IsTwoFactorEnabled { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the user has verified two-factor authentication.
    /// </summary>
    public bool IsTwoFactorVerified { get; set; } = false;

    /// <summary>
    /// Gets or sets the secret used for two-factor authentication.
    /// </summary>
    public string TwoFactorSecret { get; set; } = "";

    /// <summary>
    /// Gets or sets the position of the user within the organization.
    /// </summary>
    
    [ForeignKey("UserProfileId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual UserProfileAggregate UserProfile { get; set; } = null!;
    public List<IdentityUserToken<int>> UserToken { get; set; } = new List<IdentityUserToken<int>>();

}
