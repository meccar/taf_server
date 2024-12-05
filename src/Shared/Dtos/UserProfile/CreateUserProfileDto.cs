using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Dtos.UserProfile;

public class CreateUserProfileDto
{
    /// <summary>
    /// User's FirstName
    /// </summary>
    /// <example>John</example>
    [Required] 
    [StringLength(255)]
    [SwaggerSchema("FirstName details")]

    public string FirstName { get; set; } = "";
    
    /// <summary>
    /// User's LastName
    /// </summary>
    /// <example>Smith</example>
    [Required] 
    [StringLength(255)] 
    [SwaggerSchema("LastName details")]

    public string LastName { get; set; } = "";

    /// <summary>
    /// User's Gender
    /// </summary>
    /// <example>Male</example>
    [Required] 
    [SwaggerSchema("Gender details")]
    public string Gender { get; set; } = "";
    
    /// <summary>
    /// User's DateOfBirth
    /// </summary>
    /// <example>07/14/1999</example>
    [Required]
    [SwaggerSchema("DateOfBirth details")]
    public string DateOfBirth { get; set; } = "";
}