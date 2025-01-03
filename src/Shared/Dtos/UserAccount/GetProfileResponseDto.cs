using Shared.Dtos.UserProfile;

namespace Shared.Dtos.UserAccount;

public class GetProfileResponseDto
{
    /// <summary>
    /// Gets or sets the unique employee identifier (Eid) for the user.
    /// This field is required and represents the user's employee ID or another unique account identifier.
    /// </summary>
    public required string Eid { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user's account.
    /// This field is required and represents the user's contact email.
    /// </summary>
    public required string Email { get; set; }
    /// <summary>
    /// Gets or sets the user profile details associated with the registered user.
    /// This field is optional and may be null if no profile details
    /// </summary>
    public UserProfileResponseDto? UserProfile { get; set; }
}