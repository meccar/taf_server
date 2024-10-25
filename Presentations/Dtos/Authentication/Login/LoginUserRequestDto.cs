using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace taf_server.Presentations.Dtos.Authentication.Login;

public class LoginUserRequestDto
{
    [Required]
    [SwaggerSchema("Email details")]
    public string Email { get; set; }
    [Required]
    [SwaggerSchema("Password details")]
    public string Password { get; set; }
}