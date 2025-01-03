namespace Shared.Dtos.UserProfile;

public class UpdateUserProfileRequestDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public required string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Avatar { get; set; }
}