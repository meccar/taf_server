using Application.Dtos.UserAccount;
using Application.Dtos.UserLoginData;

namespace Application.Dtos.Authentication.Register;

public class RegisterUserResponseDto
{
    // public UserAccountResponseDto UserAccount { get; set; }
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    // public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public string Status { get; set; }
    public UserLoginDataResponseDto? UserLoginData { get; set; }
    // public CompanyResponseDto Company { get; set; }
}