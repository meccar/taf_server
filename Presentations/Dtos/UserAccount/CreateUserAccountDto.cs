using System.ComponentModel.DataAnnotations;
using taf_server.Domain.SeedWork.Enums.UserAccount;

namespace taf_server.Presentations.Dtos.UserAccount;

public class CreateUserAccountDto
{
    [Required] 
    [StringLength(255)]
    public string Uuid { get; set; } = "";
    public string FirstName { get; set; } = "";
    
    [Required] 
    [StringLength(255)] 
    public string LastName { get; set; } = "";

    [Required] 
    public string Gender { get; set; } = "";
    
    [Required]
    public string DateOfBirth { get; set; } = "";
    
    [Required] 
    [Phone] 
    public string PhoneNumber { get; set; } = "";
}