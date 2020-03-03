using System.Security.Claims;

namespace SadWave.Events.Api.Common.Authentication
{
	public static class ClaimsIdentityUtils
	{
		public static ClaimsIdentity Create(string name, string role)
		{
			var claims = new[]
			{
				new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
				new Claim(ClaimsIdentity.DefaultNameClaimType, name)
			};

			return new ClaimsIdentity(
				claims,
				"Token",
				ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
		}
	}
}
