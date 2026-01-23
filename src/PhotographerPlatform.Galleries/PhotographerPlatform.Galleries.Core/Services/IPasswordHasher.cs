namespace PhotographerPlatform.Galleries.Core.Services;

public interface IPasswordHasher
{
    string Hash(string password, string salt);
    bool Verify(string password, string salt, string hash);
    string GenerateSalt();
}
