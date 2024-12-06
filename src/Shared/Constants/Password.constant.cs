namespace Shared.Constants;

/// <summary>
/// Contains regular expression patterns for password validation rules.
/// </summary>
public class PasswordConstants
{
    /// <summary>
    /// Regular expression to ensure a password contains at least one uppercase letter.
    /// </summary>
    public static readonly string Uppercase = @"^(?=.*[A-Z])";

    /// <summary>
    /// Regular expression to ensure a password contains at least one lowercase letter.
    /// </summary>
    public static readonly string Lowercase = @"^(?=.*[a-z])";

    /// <summary>
    /// Regular expression to ensure a password contains at least one digit.
    /// </summary>
    public static readonly string Digit = @"^(?=.*\d)";

    /// <summary>
    /// Regular expression to ensure a password contains at least one special character.
    /// </summary>
    public static readonly string SpecialCharacter = @"^(?=.*[!@#$%^&*()_+{}[\]:;<>,.?~\\/-])";
}