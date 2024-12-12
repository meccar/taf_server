namespace Shared.Model;

/// <summary>
/// Represents the profile of a user.
/// </summary>
public class UserProfileModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the user profile.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the external identifier of the user profile.
    /// </summary>
    public string EId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    public string Gender { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    public string DateOfBirth { get; set; } = null!;

    /// <summary>
    /// Gets or sets the avatar of the user.
    /// </summary>
    public string Avatar { get; set; } = "";

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    public string Status { get; set; } = null!;

    // public int CompanyId { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date and time of the user profile.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the user profile.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the deletion date and time of the user profile.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// Whether user's profile is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
    
    // public UserLoginDataExternalEntity UserLoginDataExternal { get; set; }
    
    /// <summary>
    /// Gets or sets the associated user account, if any.
    /// </summary>
    public UserAccountModel? UserAccount { get; set; }
    
    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    // public List<UserTokenModel> Tokens { get; set; }
    // public List<RoleModel> Roles { get; set; }
    // public CompanyModel Company { get; set; }

}
