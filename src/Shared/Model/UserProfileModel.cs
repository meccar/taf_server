using Shared.Enums;

namespace Shared.Model;

/// <summary>
/// 
/// </summary>
public class UserProfileModel
{
    public string Id { get; set; }
    public string EId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public EGender Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Avatar { get; set; } = "";
    public string Status { get; set; }
    // public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    // public UserLoginDataExternalEntity UserLoginDataExternal { get; set; }
    public UserAccountModel? UserAccount { get; set; }
    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    // public List<UserTokenModel> Tokens { get; set; }
    // public List<RoleModel> Roles { get; set; }
    // public CompanyModel Company { get; set; }

}
