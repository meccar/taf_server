using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Dtos.Authentication.Login;

/// <summary>
/// Represents a request to Login.
/// </summary>
public class LoginUserRequestDto
{
    /// <summary>
    /// User's Email
    /// </summary>
    /// <example>john.smith@gmail.com</example>
    [Required]
    [SwaggerSchema("Email details")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// User's Password
    /// </summary>
    /// <example>Password@1234</example>
    [Required]
    [SwaggerSchema("Password details")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Remember User
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema("Remember User details")]
    public bool RememberUser { get; set; }
}