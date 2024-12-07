﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Domain.Aggregates;

/// <summary>
/// Represents a user account within the application, extending the IdentityUser class.
/// </summary>
/// <remarks>
/// This aggregate encapsulates user-related data and behavior, including personal details,
/// account status, and associated tokens. It implements the <see cref="IDateTracking"/> 
/// interface for managing creation and update timestamps.
/// </remarks>
public class UserProfileAggregate : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Id { get; set; }
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string EId { get; set; } = Ulid.NewUlid().ToString();
    
    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    public string FirstName { get; set; } = "";
    
    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    public string LastName { get; set; } = "";

    [Required] public EGender Gender { get; set; }

    [Required]
    [ProtectedPersonalData]
    [PersonalData]
    public string DateOfBirth { get; set; }

    [Required]
    public string Avatar { get; set; } = null;
    
    public string Status { get; set; } = EUserAccountStatus.Inactive.ToString();
    
    // public int CompanyId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
   
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    // public UserLoginDataExternalEntity? UserLoginDataExternal { get; set; }

    public virtual UserAccountAggregate UserAccount { get; set; } = null!;

    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }


    // public List<RoleModel> Roles { get; set; }

    // public CompanyModel Company { get; set; }
}
