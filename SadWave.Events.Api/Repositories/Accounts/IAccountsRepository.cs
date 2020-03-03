using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories.Accounts
{
	public interface IAccountsRepository
	{
		Task CreateAsync(string login, byte[] hash, string role);

		Task<bool> DoesExistAsync(string login);

		Task<AccountRecord> GetAsync(string login);
	}
}