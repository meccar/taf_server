using System.Security.Cryptography;

namespace Infrastructure.Extensions;

/// <summary>
/// Defines supported protector algorithms.
/// </summary>
public enum ProtectorAlgorithm
{
    /// <summary>
    /// AES-256 encryption with HMAC-SHA-512 for signing.
    /// </summary>
    Aes256Hmac512 = 1
}

/// <summary>
/// Provides helper methods for handling protector algorithms.
/// </summary>
public static class ProtectorAlgorithmHelper
{
    /// <summary>
    /// Gets the default protector algorithm.
    /// </summary>
    public static ProtectorAlgorithm DefaultAlgorithm
    {
        get { return ProtectorAlgorithm.Aes256Hmac512; }
    }

    /// <summary>
    /// Retrieves the encryption algorithm, signing algorithm, and key derivation iteration count for a given protector algorithm.
    /// </summary>
    /// <param name="algorithmId">The identifier of the protector algorithm.</param>
    /// <param name="encryptionAlgorithm">The symmetric encryption algorithm.</param>
    /// <param name="signingAlgorithm">The keyed-hash signing algorithm.</param>
    /// <param name="keyDerivationIterationCount">The number of iterations for key derivation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the algorithmId is not supported.</exception>
    public static void GetAlgorithms(
        ProtectorAlgorithm algorithmId,
        out SymmetricAlgorithm encryptionAlgorithm,
        out KeyedHashAlgorithm signingAlgorithm,
        out int keyDerivationIterationCount)
    {
        switch (algorithmId)
        {
            case ProtectorAlgorithm.Aes256Hmac512:
                encryptionAlgorithm = Aes.Create();
                encryptionAlgorithm.KeySize = 256;
                signingAlgorithm = new HMACSHA512();
                keyDerivationIterationCount = 10000;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(algorithmId));
        }
    }
}