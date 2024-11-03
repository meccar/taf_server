using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Dtos.Authentication.Login;

public class LoginUserRequestDto
{
    [Required]
    [SwaggerSchema("Email details")]
    public string Email { get; set; }
    [Required]
    [SwaggerSchema("Password details")]
    public string Password { get; set; }
}