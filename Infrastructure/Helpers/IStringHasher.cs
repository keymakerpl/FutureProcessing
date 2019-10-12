namespace Infrastructure.Helpers
{
    public interface IStringHasher
    {
        void GenerateSaltedHash(string input, out string hash, out string salt);
        bool VerifyString(string enteredString, string storedHash, string storedSalt);
    }
}