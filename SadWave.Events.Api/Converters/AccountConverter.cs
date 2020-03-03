using SadWave.Events.Api.Repositories.Accounts;
using SadWave.Events.Api.Services.Accounts;

namespace SadWave.Events.Api.Converters
{
	public static class AccountConverter
	{
		public static Account Convert(AccountRecord record)
		{
			if (record == null)
				return null;

			return new Account
			{
				Login = record.Login,
				Role = RoleUtils.GetRole(record.Role)
			};
		}
	}
}