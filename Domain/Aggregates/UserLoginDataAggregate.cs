using Domain.SeedWork.Enums.UserLoginData;
using Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates;

/// <summary>
/// Represents the login data associated with a user account.
/// </summary>
/// <remarks>
/// This aggregate extends the IdentityUser class and encapsulates data related to
/// user authentication, including password management, email status, and user 
/// account association. It implements the <see cref="IDateTracking"/> interface 
/// for tracking the creation and update timestamps.
/// </remarks>
public class UserLoginDataAggregate : IdentityUser<int>, IDateTracking
{
    public new int Id { get; set; }
    public string Uuid { get; set; } = "";
    public int UserAccountId { get; set; }
    public new string PasswordHash { get; set; } = "";
    public new string Email { get; set; } = "";
    public EmailStatus EmailStatus { get; set; }
    public string PasswordRecoveryToken { get; set; }
    public string ConfirmationToken { get; set; } = "";
    public bool IsTwoFactorEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public UserPosition UserPosition { get; set; }
    // public virtual ICollection<UserAccountAggregate> UserAccount { get; set; } = new List<UserAccountAggregate>();
    public virtual UserAccountAggregate UserAccount { get; set; }

    // public UserAccountModel UserAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}