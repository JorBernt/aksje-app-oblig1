using aksjeapp_backend.DAL;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace aksjeapp_backend.Security
{
    public class Security
    {
        private readonly StockContext _db;

        private readonly string _loggedIn = "LoggedIn";
        public Security(StockContext db)
        {
            _db = db;
        }

        public static byte[] GenHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] GenSalt()
        {
            var csp = RandomNumberGenerator.Create();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<bool> LogIn(Customer user, ISession session)
        {
            try
            {
                var userFound = await _db.Users.FirstOrDefaultAsync(b => b.Username == user.SocialSecurityNumber);
                // sjekk passordet
                byte[] hash = GenHash(user.Password, userFound.Salt);
                bool ok = hash.SequenceEqual(userFound.Password);
                if (ok)
                {
                    session.SetString(_loggedIn, user.SocialSecurityNumber);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<string> LoggedIn(ISession session)
        {
            if (session.GetString(_loggedIn) != null)
            {
                return session.GetString(_loggedIn);
            }
            return null;
        }
    }
}
