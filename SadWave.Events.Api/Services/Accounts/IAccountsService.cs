using System.Threading.Tasks;

namespace SadWave.Events.Api.Services.Accounts
{
	public interface IAccountsService
	{
		Task CreateAsync(string login, string password, Role role);

		Task<Account> GetAsync(string login, string password);
	}
}