using System.Security.Cryptography;
using System.Text;

namespace API.Utils;

public static class PasswordHelper
{
    public static (string Hash, string Salt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var saltBytes = hmac.Key;
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        string salt = Convert.ToBase64String(saltBytes);
        string hash = Convert.ToBase64String(hashBytes);
        return (hash, salt);
    }

    public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = Convert.FromBase64String(storedHash);

        using var hmac = new HMACSHA512(saltBytes);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(hashBytes);
    }
}
