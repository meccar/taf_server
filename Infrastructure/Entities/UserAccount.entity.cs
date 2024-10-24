using taf_server.Domain.Entities;
using taf_server.Domain.SeedWork.Enums.UserAccount;

namespace taf_server.Infrastructure.Entities;
public class UserAccountEntity : EntityBase
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string Avatar { get; set; } = "";
    // public UserAccountStatus Status { get; set; }
    // public int CompanyId { get; set; }
    // public UserLoginDataExternalEntity[] UserLoginExternals { get; set; }
    public UserLoginDataEntity? UserLoginData { get; set; }
    // public BlacklistTokenEntity[] BlackListTokens { get; set; }
    // public UserTokenEntity[] Tokens { get; set; }
    // public RoleEntity Roles { get; set; }
    // public CompanyEntity Company { get; set; }
}
