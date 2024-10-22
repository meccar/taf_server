using taf_server.Domain.Model;
using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Entities;
using taf_server.Domain.SeedWork.Enums.UserAccount;
using taf_server.Domain.SeedWork.Interfaces;
using taf_server.Infrastructure.Entities;

namespace taf_server.Domain.Aggregates;

/// <summary>
/// Represents a user account within the application, extending the IdentityUser class.
/// </summary>
/// <remarks>
/// This aggregate encapsulates user-related data and behavior, including personal details,
/// account status, and associated tokens. It implements the <see cref="IDateTracking"/> 
/// interface for managing creation and update timestamps.
/// </remarks>
public class UserAccountAggregate : IdentityUser, IDateTracking
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public UserAccountStatus Status { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public UserLoginDataExternalEntity UserLoginDataExternal { get; set; }
    public UserLoginDataModel UserLoginData { get; set; }
    public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    public List<UserTokenModel> Tokens { get; set; }
    public List<RoleModel> Roles { get; set; }
    public CompanyModel Company { get; set; }
    public bool IsDeleted { get; set; } = false;
}
