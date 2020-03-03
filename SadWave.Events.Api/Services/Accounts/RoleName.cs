namespace SadWave.Events.Api.Services.Accounts
{
	public static class RoleName
	{
		public const string Admin = "admin";

		public const string User = "user";

		public static bool DoesExist(string role)
		{
			return role == Admin || role == User;
		}
	}
}
