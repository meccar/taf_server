using taf_server.Domain.SeedWork.Enums.UserAccount;

namespace taf_server.Presentations.Dtos.UserAccount;

public class UserAccountResponseDto
{
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public UserAccountStatus Status { get; set; }
    public string Email { get; set; }
}