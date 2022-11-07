using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Cryptography;

namespace aksjeapp_backend.DAL
{
    public class SecurityRepository
{
        private readonly StockContext _db;

        public SecurityRepository(StockContext db)
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

        public async Task<bool> LoggInn(Customer user)
        {
            try
            {
                var userFound = await _db.Users.FirstOrDefaultAsync(b => b.Username == user.SocialSecurityNumber);
                // sjekk passordet
                byte[] hash = GenHash(user.Password, userFound.Salt);
                bool ok = hash.SequenceEqual(userFound.Password);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
}
