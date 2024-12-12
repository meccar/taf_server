using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Domain.Aggregates;

/// <summary>
/// Represents the user profile aggregate in the domain model.
/// This class encapsulates all personal information related to a user, such as their name, 
/// gender, date of birth, and avatar, as well as associated account status and deletion information.
/// It is part of the user profile domain logic and is used to maintain consistency 
/// of user-related data in the system.
/// </summary>
public class UserProfileAggregate : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the user profile.
    /// This is the primary key for the user profile entity.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique external identifier for the user profile.
    /// This value is automatically generated and is intended to be unique across the system.
    /// </summary>
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string EId { get; set; } = Ulid.NewUlid().ToString();
    
    /// <summary>
    /// Gets or sets the user's first name.
    /// This value is required and is considered protected personal data.
    /// </summary>
    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    [MaxLength(50)]
    public string FirstName { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the user's last name.
    /// This value is required and is considered protected personal data.
    /// </summary>
    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    [MaxLength(50)]
    public string LastName { get; set; } = "";

    /// <summary>
    /// Gets or sets the user's gender.
    /// This value is required and represents the gender of the user.
    /// </summary>
    [Required]
    [MaxLength(6)]
    public required string Gender { get; set; }
    // public EGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the user's date of birth.
    /// This value is required and is considered protected personal data.
    /// </summary>
    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    [MaxLength(10)]
    public string DateOfBirth { get; set; } = "";

    /// <summary>
    /// Gets or sets the user's avatar URL.
    /// This value is required and represents the URL of the user's profile picture.
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Avatar { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the current status of the user.
    /// This value represents the user's account status (e.g., "Active", "Inactive").
    /// </summary>
    [MaxLength(8)]
    public string Status { get; set; } = EUserAccountStatus.Inactive.ToString();
    
    // public int CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the associated user account for this profile.
    /// This is a required relationship between the profile and the user's account.
    /// </summary>
    public virtual UserAccountAggregate UserAccount { get; set; } = null!;

    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }


    // public List<RoleModel> Roles { get; set; }

    // public CompanyModel Company { get; set; }
}
