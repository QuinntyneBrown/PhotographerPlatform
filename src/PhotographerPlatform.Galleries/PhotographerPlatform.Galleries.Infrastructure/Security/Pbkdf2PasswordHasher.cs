using System.Security.Cryptography;
using PhotographerPlatform.Galleries.Core.Services;

namespace PhotographerPlatform.Galleries.Infrastructure.Security;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public string GenerateSalt()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(SaltSize)).ToLowerInvariant();
    }

    public string Hash(string password, string salt)
    {
        var saltBytes = Convert.FromHexString(salt);
        using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
        var key = deriveBytes.GetBytes(KeySize);
        return Convert.ToHexString(key).ToLowerInvariant();
    }

    public bool Verify(string password, string salt, string hash)
    {
        var expected = Hash(password, salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromHexString(expected),
            Convert.FromHexString(hash));
    }
}
