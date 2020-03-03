using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SadWave.Events.Api.Repositories
{
	public class ConnectionFactory : IConnectionFactory<SqliteConnection>
	{
		private readonly string _databasePath;

		public ConnectionFactory(string databasePath)
		{
			_databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
		}

		public async Task<SqliteConnection> CreateConnectionAsync()
		{
			var connection = Create();
			await connection.OpenAsync();
			return connection;
		}

		public SqliteConnection CreateConnection()
		{
			var connection = Create();
			connection.Open();
			return connection;
		}

		private SqliteConnection Create()
		{
			return new SqliteConnection($"Data Source={ _databasePath }");
		}
	}
}
