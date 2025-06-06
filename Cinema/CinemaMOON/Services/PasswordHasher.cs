using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CinemaMOON.Services
{
	public static class PasswordHasher
	{
		public static (string Hash, string Salt) HashPassword(string password)
		{
			byte[] saltBytes = new byte[64];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(saltBytes);
			}
			string salt = Convert.ToBase64String(saltBytes);

			using var sha256 = SHA256.Create();
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
			byte[] hashBytes = sha256.ComputeHash(passwordBytes);
			string hash = Convert.ToBase64String(hashBytes);

			return (hash, salt);
		}

		public static bool VerifyPassword(string password, string storedHash, string storedSalt)
		{
			using var sha256 = SHA256.Create();
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password + storedSalt);
			byte[] hashBytes = sha256.ComputeHash(passwordBytes);
			string computedHash = Convert.ToBase64String(hashBytes);

			return computedHash == storedHash;
		}
	}
}
