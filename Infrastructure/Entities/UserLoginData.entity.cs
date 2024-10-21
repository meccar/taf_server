using taf_server.Domain.Entities;
using taf_server.Domain.SeedWork.Enums.UserLoginData;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class UserLoginDataEntity : EntityBase
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public int UserAccountId { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public required EmailStatus EmailStatus { get; set; }
    public required string PasswordRecoveryToken { get; set; }
    public required string ConfirmationToken { get; set; }
    public bool IsTwoFactoeEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public required string TwoFactorSecret { get; set; }
    public required UserPosition UserPosition { get; set; }
    //public required BaseEntity baseEntity { get; set; }
    public required UserAccountEntity UserAccount { get; set; }
}
