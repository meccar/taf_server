using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Share.Dtos.Authentication.Login;

public class LoginUserRequestDto
{
    /// <summary>
    /// User's Email
    /// </summary>
    /// <example>john.smith@gmail.com</example>
    [Required]
    [SwaggerSchema("Email details")]
    public string Email { get; set; }
    
    /// <summary>
    /// User's Password
    /// </summary>
    /// <example>Password@1234</example>
    [Required]
    [SwaggerSchema("Password details")]
    public string Password { get; set; }

    /// <summary>
    /// Remember User
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema("Remember User details")]
    public bool RememberUser { get; set; }
}