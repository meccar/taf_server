using taf_server.Domain.Entities;
using taf_server.Domain.SeedWork.Enums.UserAccount;

namespace taf_server.Infrastructure.Entities;
public class UserAccountEntity : EntityBase
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
    //public required BaseEntity baseEntity { get; set; }
    public required UserLoginDataExternalEntity[] UserLoginExternals { get; set; }
    public required UserLoginDataEntity UserLoginData { get; set; }
    public required BlacklistTokenEntity[] BlackListTokens { get; set; }
    public required UserTokenEntity[] Tokens { get; set; }
    public required RoleEntity Roles { get; set; }
    public required CompanyEntity Company { get; set; }
}
