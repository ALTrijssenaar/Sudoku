namespace Sudoku.Core.Security;

public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The plaintext password to hash</param>
    /// <returns>The hashed password</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies that a password matches a hash.
    /// </summary>
    /// <param name="hashedPassword">The hashed password</param>
    /// <param name="providedPassword">The plaintext password to verify</param>
    /// <returns>True if the password matches, false otherwise</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
