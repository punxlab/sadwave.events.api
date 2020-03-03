using System;
using Microsoft.IdentityModel.Tokens;

namespace SadWave.Events.Api.Common.Authentication
{
	public class AuthenticationOptions
	{
		public AuthenticationOptions(
			SymmetricSecurityKey key,
			TimeSpan? tokenLifeTime,
			string issuer)
		{
			if (string.IsNullOrWhiteSpace(issuer))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(issuer));
			Key = key ?? throw new ArgumentNullException(nameof(key));
			TokenLifeTime = tokenLifeTime;
			Issuer = issuer;
		}

		public SymmetricSecurityKey Key { get; }

		public TimeSpan? TokenLifeTime { get; }

		public string Issuer { get; }
	}
}
