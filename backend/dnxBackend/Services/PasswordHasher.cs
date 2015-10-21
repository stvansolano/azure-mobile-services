namespace Backend
{
    using System;
    using Microsoft.AspNet.Identity;

    public class ClearPassword : IPasswordHasher<User>
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public string HashPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword.Equals(providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }
}