using System.ComponentModel.DataAnnotations;

namespace Share.Dtos.UserAccount;

public class CreateUserAccountDto
{
    /// <summary>
    /// User's FirstName
    /// </summary>
    /// <example>John</example>
    [Required] 
    [StringLength(255)]
    public string FirstName { get; set; } = "";
    
    /// <summary>
    /// User's LastName
    /// </summary>
    /// <example>Smith</example>
    [Required] 
    [StringLength(255)] 
    public string LastName { get; set; } = "";

    /// <summary>
    /// User's Gender
    /// </summary>
    /// <example>Male</example>
    [Required] 
    public string Gender { get; set; } = "";
    
    /// <summary>
    /// User's DateOfBirth
    /// </summary>
    /// <example>07/14/1999</example>
    [Required]
    public string DateOfBirth { get; set; } = "";
}