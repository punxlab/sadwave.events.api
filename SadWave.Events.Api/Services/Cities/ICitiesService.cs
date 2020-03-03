using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Services.Cities
{
	public interface ICitiesService
	{
		Task<bool> DoesExistAsync(string alias);

		Task<City> GetAsync(string alias);

		Task<City> GetAsync(int id);

		Task<IEnumerable<City>> GetAsync();

		Task AddAsync(string alias, string name, string uri);
	}
}