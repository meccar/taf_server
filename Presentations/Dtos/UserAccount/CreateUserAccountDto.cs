using System.ComponentModel.DataAnnotations;

namespace taf_server.Presentations.Dtos.UserAccount;

public class CreateUserAccountDto
{
    [Key]
    public Ulid Uuid { get; set; } = Ulid.NewUlid();
    
    [Required] 
    [StringLength(255)]
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