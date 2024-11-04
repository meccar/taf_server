using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserLoginData;

public class CreateUserLoginDataDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength =12)]
    public string Password { get; set; }
}