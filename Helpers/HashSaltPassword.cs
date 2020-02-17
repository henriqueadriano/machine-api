using machine_api.Models.User;
using System;

namespace machine_api.Helpers
{
    public class HashSaltPassword
    {
        public static User CreatePasswordHash(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            return user;
        }
    }
}
