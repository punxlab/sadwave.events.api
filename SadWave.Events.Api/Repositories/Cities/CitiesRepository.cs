using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SadWave.Events.Api.Repositories.Cities
{
	public class CitiesRepository : ICitiesRepository
	{
		private readonly IConnectionFactory<SqliteConnection> _connectionFactory;

		public CitiesRepository(IConnectionFactory<SqliteConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public async Task<CityRecord> GetAsync(string alias)
		{
			if (string.IsNullOrWhiteSpace(alias))
				return null;

			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleOrDefaultAsync<CityRecord>(
@"SELECT
	C.Id AS Id,
	C.Alias AS Alias,
	C.Name AS Name,
	C.Uri AS PageUrl
FROM Cities C
WHERE
	C.Alias = @alias",
					new { alias });
			}
		}

		public async Task<CityRecord> GetAsync(int id)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleOrDefaultAsync<CityRecord>(
@"SELECT
	C.Id AS Id,
	C.Alias AS Alias,
	C.Name AS Name,
	C.Uri AS PageUrl
FROM Cities C
WHERE
	C.Id = @id",
					new { id });
			}
		}

		public async Task<IEnumerable<CityRecord>> GetAsync()
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QueryAsync<CityRecord>(
@"SELECT
	C.Id AS Id,
	C.Alias AS Alias,
	C.Name AS Name,
	C.Uri AS PageUrl
FROM Cities C");
			}
		}

		public Task AddAsync(string alias, string name, string uri)
		{
			if (string.IsNullOrWhiteSpace(alias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(uri));
			if(!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				throw new ArgumentException("Value should be an absolute correct format uri.", nameof(uri));

			return AddCiyAsync(alias, name, uri);
		}

		public Task<bool> DoesExistAsync(string alias)
		{
			if (string.IsNullOrWhiteSpace(alias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));

			return DoesCityExistAsync(alias);
		}

		private async Task<bool> DoesCityExistAsync(string alias)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleAsync<bool>(
					@"SELECT EXISTS(SELECT 1 FROM Cities WHERE Alias = @alias)",
					new { alias });
			}
		}

		private async Task AddCiyAsync(string alias, string name, string uri)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"INSERT OR REPLACE INTO Cities (Alias, Name, Uri)
VALUES (@alias, @name, @uri)",
					new
					{
						alias,
						name,
						uri
					});
			}
		}
	}
}