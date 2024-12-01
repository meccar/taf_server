using Share.Enums;

namespace Share.Dtos.UserAccount;

public class UserAccountResponseDto
{
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EGender Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Avatar { get; set; }
}
