using Domain.SeedWork.Enums.UserAccount;

namespace Domain.Model;
/// <summary>
/// 
/// </summary>
public class UserAccountModel
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string Avatar { get; set; } = "";
    public UserAccountStatus Status { get; set; }
    // public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    // public UserLoginDataExternalEntity UserLoginDataExternal { get; set; }
    public UserLoginDataModel? UserLoginData { get; set; }
    // public List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    // public List<UserTokenModel> Tokens { get; set; }
    // public List<RoleModel> Roles { get; set; }
    // public CompanyModel Company { get; set; }

}
