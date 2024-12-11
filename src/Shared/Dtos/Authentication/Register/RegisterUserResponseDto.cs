using Shared.Dtos.UserAccount;

namespace Shared.Dtos.Authentication.Register;

/// <summary>
/// Represents the response DTO for a newly registered user, containing the user's personal details such as their name, gender, 
/// date of birth, optional avatar, and associated user account information.
/// This class is used to return the registered user details after a successful registration process.
/// </summary>
public class RegisterUserResponseDto
{
    /// <summary>
    /// Gets or sets the unique employee identifier (Eid) for the user.
    /// This field is required and represents the user's unique employee ID or another type of identifier.
    /// </summary>
    public required string Eid { get; set; }
    
    /// <summary>
    /// Gets or sets the user's first name.
    /// This field is required and represents the user's given name.
    /// </summary>
    public required string FirstName { get; set; }
    
    /// <summary>
    /// Gets or sets the user's last name.
    /// This field is required and represents the user's surname or family name.
    /// </summary>
    public required string LastName { get; set; }
    
    /// <summary>
    /// Gets or sets the user's gender.
    /// This field is required and represents the gender identity of the user (e.g., "Male", "Female", "Non-binary").
    /// </summary>
    public required string Gender { get; set; }
    
    /// <summary>
    /// Gets or sets the user's date of birth.
    /// This field is required and represents the user's birthdate, typically in a standard date format (e.g., "yyyy-MM-dd").
    /// </summary>
    public required string DateOfBirth { get; set; }
    
    // public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the URL of the user's avatar image.
    /// This field is optional and may be null if the user has not set an avatar.
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// Gets or sets the user account details associated with the registered user.
    /// This field is optional and may be null if no account details are provided at the time of registration.
    /// </summary>
    public UserAccountResponseDto? UserAccount { get; set; }
    // public CompanyResponseDto Company { get; set; }
}