namespace Shared.Dtos.UserProfile;

/// <summary>
/// Represents the response DTO for a user profile, containing personal and optional avatar information.
/// This DTO is used to transfer user profile data from the backend to the client.
/// </summary>
public class UserProfileResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// This field is required and represents a globally unique identifier (UUID) for the user.
    /// </summary>
    public required string Uuid { get; set; }

    /// <summary>
    /// Gets or sets the user's first name.
    /// This field is required and represents the user's given name.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// This field is required and represents the user's family name or surname.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the user's gender.
    /// This field is required and represents the gender identity of the user (e.g., "Male", "Female", "Non-binary", etc.).
    /// </summary>
    public required string Gender { get; set; }

    /// <summary>
    /// Gets or sets the user's date of birth.
    /// This field is required and represents the user's birthdate in a standard date format (e.g., "yyyy-MM-dd").
    /// </summary>
    public required string DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the URL of the user's avatar image.
    /// This field is optional and may be null if no avatar is set for the user.
    /// </summary>
    public string? Avatar { get; set; }
}
