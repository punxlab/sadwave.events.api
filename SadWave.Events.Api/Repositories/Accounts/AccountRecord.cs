namespace SadWave.Events.Api.Repositories.Accounts
{
	public class AccountRecord
	{
		public string Login { get; set; }

		public byte[] Password { get; set; }

		public string Role { get; set; }
	}
}