using Shared.Enums;

namespace Shared.Dtos.UserProfile;

public class UserProfileResponseDto
{
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EGender Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Avatar { get; set; }
}
