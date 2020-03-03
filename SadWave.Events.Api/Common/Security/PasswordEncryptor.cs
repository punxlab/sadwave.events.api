using System;
using System.Security.Cryptography;

namespace SadWave.Events.Api.Common.Security
{
	public class PasswordEncryptor : IPasswordEncryptor
	{
		private const int SaltSize = 16;
		private const int HashSize = 20;
		private const int HashIterations = 64000;

		public byte[] Hash(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

			using (var cryptoProvider = new RNGCryptoServiceProvider())
			{
				var salt = new byte[SaltSize];
				cryptoProvider.GetBytes(salt);

				using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, HashIterations))
				{
					var hash = deriveBytes.GetBytes(HashSize);
					return CreateHashWithSalt(hash, salt);
				}
			}
		}

		public bool Verify(byte[] hashedPassword, string verifingPassword)
		{
			if (hashedPassword == null)
				throw new ArgumentNullException(nameof(hashedPassword));
			if (string.IsNullOrWhiteSpace(verifingPassword))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(verifingPassword));

			var salt = GetSalt(hashedPassword);
			var hash = GetHash(hashedPassword);

			using (var deriveBytes = new Rfc2898DeriveBytes(verifingPassword, salt, HashIterations))
			{
				var verifyingHash = deriveBytes.GetBytes(HashSize);
				for (var i = 0; i < HashSize; i++)
				{
					if (verifyingHash[i] != hash[i])
						return false;
				}
			}

			return true;
		}

		private static byte[] CreateHashWithSalt(byte[] hash, byte[] salt)
		{
			var hashBytes = new byte[SaltSize + HashSize];
			Array.Copy(salt, 0, hashBytes, 0, SaltSize);
			Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
			return hashBytes;
		}

		private byte[] GetHash(byte[] hashWithSalt)
		{
			var hash = new byte[HashSize];
			Array.Copy(hashWithSalt, SaltSize, hash, 0, HashSize);
			return hash;
		}

		private byte[] GetSalt(byte[] hashWithSalt)
		{
			var salt = new byte[SaltSize];
			Array.Copy(hashWithSalt, 0, salt, 0, SaltSize);
			return salt;
		}
	}
}
