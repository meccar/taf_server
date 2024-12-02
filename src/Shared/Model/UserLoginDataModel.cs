namespace Shared.Model;

public class UserLoginDataModel
{
    public string Id { get; set; }
    public string EId { get; set; }
    public string UserAccountId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string? SecurityStamp { get; set; }
    public string Email { get; set; }
    public string EmailStatus { get; set; }
    public string PasswordRecoveryToken { get; set; }
    public string ConfirmationToken { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public UserAccountModel UserAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}