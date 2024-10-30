using Domain.SeedWork.Enums.UserAccount;
using Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates;

/// <summary>
/// Represents a user account within the application, extending the IdentityUser class.
/// </summary>
/// <remarks>
/// This aggregate encapsulates user-related data and behavior, including personal details,
/// account status, and associated tokens. It implements the <see cref="IDateTracking"/> 
/// interface for managing creation and update timestamps.
/// </remarks>
public class UserAccountAggregate : IdentityUser<int>, IDateTracking
{
    public new int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public new string PhoneNumber { get; set; } = "";

    public string Avatar { get; set; } = "";
    // public UserAccountStatus Status { get; set; }
    // public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    // public UserLoginDataExternalEntity? UserLoginDataExternal { get; set; }
    // public virtual ICollection<UserLoginDataAggregate> UserLoginData { get; set; } = new List<UserLoginDataAggregate>();
    public virtual UserLoginDataAggregate UserLoginData { get; set; }

    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    // public List<UserTokenModel> Tokens { get; set; }
    // public List<RoleModel> Roles { get; set; }
    // public CompanyModel Company { get; set; }
    public bool IsDeleted { get; set; } = false;
}
