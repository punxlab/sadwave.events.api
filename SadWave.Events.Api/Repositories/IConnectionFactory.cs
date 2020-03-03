using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories
{
	public interface IConnectionFactory<TConnection>
	{
		TConnection CreateConnection();

		Task<TConnection> CreateConnectionAsync();
	}
}