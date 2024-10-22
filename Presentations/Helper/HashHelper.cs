using BCrypt.Net;

namespace taf_server.Presentations.Helper;

public class HashHelper
{
    private const int salt = 10;

    /// <summary>
    /// Encrypts a plain string
    /// </summary>
    /// <param name="str">The string to encrypt</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains the encrypted string.</returns>
    public static string Encrypt(string str) =>
        BCrypt.Net.BCrypt.HashPassword(str, salt );

    
    /// <summary>
    /// Compares a plain string with an encrypted string
    /// </summary>
    /// <param name="plain">The plain string</param>
    /// <param name="encrypted">The encrypted string</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a boolean indicating if the strings match.</returns>
    public static bool Compare(string plain, string encrypted) =>
        BCrypt.Net.BCrypt.Verify(plain, encrypted);
}