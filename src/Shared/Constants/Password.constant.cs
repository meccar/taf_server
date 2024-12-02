namespace Shared.Constants;

public class PasswordConstants
{
    public static readonly string Uppercase = @"^(?=.*[A-Z])";
    public static readonly string Lowercase = @"^(?=.*[a-z])";
    public static readonly string Digit = @"^(?=.*\d)";
    public static readonly string SpecialCharacter = @"^(?=.*[!@#$%^&*()_+{}[\]:;<>,.?~\\/-])";
}