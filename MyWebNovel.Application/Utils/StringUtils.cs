using System.Security.Cryptography;

namespace MyWebNovel.Application.Utils
{
    public static class StringUtils
    {
        public static string GenerateRandomString(int length)
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            var token = new string([.. bytes.Select(b => characters[b % characters.Length])]);
            return token;
        }
    }
}
