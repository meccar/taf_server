using System.ComponentModel.DataAnnotations;
using Domain.SeedWork.Enums.UserLoginData;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates;

public class UserAggregate : IdentityUser<Guid>
{
    [Required]
    public string Uuid { get; set; } = Ulid.NewUlid().ToString();
    public EmailStatus? EmailStatus { get; set; }
    public string PasswordRecoveryToken { get; set; } = "";
    public string? ConfirmationToken { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public bool IsTwoFactorVerified { get; set; }
    public string TwoFactorSecret { get; set; } = "";
}