using System.ComponentModel.DataAnnotations;

namespace taf_server.Presentations.Dtos.UserLoginData;

public class CreateUserLoginDataDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(100, MinimumLength =12)]
    public string Password { get; set; }
    [Required]
    public int UserAccountId { get; set; }
}