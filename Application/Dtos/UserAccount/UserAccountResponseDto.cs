using Domain.SeedWork.Enums.UserAccount;

namespace Application.Dtos.UserAccount;

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
