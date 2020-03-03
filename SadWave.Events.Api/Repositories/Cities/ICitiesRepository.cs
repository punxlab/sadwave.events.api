using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories.Cities
{
	public interface ICitiesRepository
	{
		Task<bool> DoesExistAsync(string alias);

		Task<CityRecord> GetAsync(string alias);

		Task<CityRecord> GetAsync(int id);

		Task<IEnumerable<CityRecord>> GetAsync();

		Task AddAsync(string alias, string name, string uri);
	}
}