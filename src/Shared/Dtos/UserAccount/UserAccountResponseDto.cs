namespace Shared.Dtos.UserAccount;

/// <summary>
/// Represents the response DTO for a user account, which includes essential account details such as employee ID (Eid) and email.
/// This class is used to transfer user account data from the backend to the client.
/// </summary>
public class UserAccountResponseDto
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
}