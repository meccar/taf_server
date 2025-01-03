namespace Shared.Dtos.UserProfile;

public class UpdateUserProfileResponseDto
{
    public string EId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public required string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Avatar { get; set; }
}