using Domain.SeedWork.Enums.UserLoginData;

namespace Domain.Model;

public class UserLoginDataModel
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public int UserAccountId { get; set; }
    public string PasswordHash { get; set; } = "";
    public string Email { get; set; }
    public EmailStatus EmailStatus { get; set; }
    public string PasswordRecoveryToken { get; set; }
    public string ConfirmationToken { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public UserPosition UserPosition { get; set; }
    public UserAccountModel UserAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}