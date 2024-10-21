using System.ComponentModel.DataAnnotations;
using taf_server.Domain.SeedWork.Enums.UserAccount;

namespace taf_server.Presentations.Dtos.UserAccount;

public class CreateUserAccountDto
{
    [Required] 
    [StringLength(50)] 
    public string FirstName { get; set; } = "";
    [Required] 
    [StringLength(50)] 
    public string LastName { get; set; } = "";
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required] 
    [Phone] 
    public string PhoneNumber { get; set; } = "";
}