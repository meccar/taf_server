using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Ulid Uuid { get; private set; } = Ulid.NewUlid();
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = "";

    public string Avatar { get; set; } = "";
    // public UserAccountStatus Status { get; set; }
    // public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    // public UserLoginDataExternalEntity? UserLoginDataExternal { get; set; }
    public virtual UserLoginDataEntity UserLoginData { get; set; }

    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    // public List<UserTokenModel> Tokens { get; set; }
    // public List<RoleModel> Roles { get; set; }
    // public CompanyModel Company { get; set; }
    public bool IsDeleted { get; set; } = false;
}
