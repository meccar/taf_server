using taf_server.Domain.SeedWork.Enums.UserAccount;
using taf_server.Infrastructure.Entities;

namespace taf_server.Domain.Model;
public class UserAccountModel
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Avatar { get; set; }
    public UserAccountStatus Status { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public required UserLoginDataExternalEntity UserLoginDataExternal { get; set; }
    public required UserLoginDataModel UserLoginData { get; set; }
    public required List<BlacklistTokenModel> BlacklistedTokens { get; set; }
    public required List<UserTokenModel> Tokens { get; set; }
    public required List<RoleModel> Roles { get; set; }
    public required CompanyModel Company { get; set; }

}
