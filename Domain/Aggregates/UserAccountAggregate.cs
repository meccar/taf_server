using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.SeedWork.Enums.UserAccount;
using Domain.SeedWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Aggregates;

/// <summary>
/// Represents a user account within the application, extending the IdentityUser class.
/// </summary>
/// <remarks>
/// This aggregate encapsulates user-related data and behavior, including personal details,
/// account status, and associated tokens. It implements the <see cref="IDateTracking"/> 
/// interface for managing creation and update timestamps.
/// </remarks>
public class UserAccountAggregate : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Uuid { get; set; } = Ulid.NewUlid().ToString();
    
    [Required]
    public string FirstName { get; set; } = "";
    
    [Required]
    public string LastName { get; set; } = "";
    
    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Avatar { get; set; } = "";
    
    // public UserAccountStatus Status { get; set; }
    
    // public int CompanyId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
   
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    // public UserLoginDataExternalEntity? UserLoginDataExternal { get; set; }

    public virtual UserLoginDataEntity UserLoginData { get; set; } = null!;

    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }

    public List<UserTokenEntity> UserToken { get; set; } = new List<UserTokenEntity>();

    // public List<RoleModel> Roles { get; set; }

    // public CompanyModel Company { get; set; }
}
