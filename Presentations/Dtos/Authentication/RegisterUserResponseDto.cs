using taf_server.Domain.SeedWork.Enums.UserAccount;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Presentations.Dtos.Authentication;

public class RegisterUserResponseDto
{
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public UserLoginDataResponseDto? UserLoginData { get; set; }
    // public CompanyResponseDto Company { get; set; }
}