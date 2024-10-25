using taf_server.Domain.Entities;
using taf_server.Domain.SeedWork.Enums.UserLoginData;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class UserLoginDataEntity : EntityBase
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public int UserAccountId { get; set; }
    public string PasswordHash { get; set; } = "";
    public string Email { get; set; } = "";
    public EmailStatus? EmailStatus { get; set; }
    public string PasswordRecoveryToken { get; set; } = "";
    public string ConfirmationToken { get; set; } = "";
    public bool IsTwoFactoeEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public string TwoFactorSecret { get; set; } = "";
    public UserPosition? UserPosition { get; set; }
    //public BaseEntity? baseEntity { get; set; }
    public UserAccountEntity? UserAccount { get; set; }
}
