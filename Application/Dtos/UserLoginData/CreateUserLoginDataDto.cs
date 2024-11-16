using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserLoginData;

public class CreateUserLoginDataDto
{
    /// <summary>
    /// User's Email
    /// </summary>
    /// <example>john.smith@gmail.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    /// <summary>
    /// User's Password
    /// </summary>
    /// <example>Password@1234</example>
    [Required]
    [StringLength(100, MinimumLength =12)]
    public string Password { get; set; }
        
    /// <summary>
    /// User's PhoneNumber
    /// </summary>
    /// <example>098765432123</example>
    [Required] 
    [Phone] 
    public string PhoneNumber { get; set; } = "";
}