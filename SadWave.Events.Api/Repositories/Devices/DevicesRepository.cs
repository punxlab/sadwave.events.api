using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SadWave.Events.Api.Repositories.Devices
{
	public class DevicesRepository : IDevicesRepository
	{
		private readonly IConnectionFactory<SqliteConnection> _connectionFactory;

		public DevicesRepository(IConnectionFactory<SqliteConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public Task AddAsync(string deviceToken, string deviceOs, bool sandbox)
		{
			return CheckParametersAndAddAsync(deviceToken, null, deviceOs, sandbox);
		}

		public Task AddAsync(string deviceToken, int cityId, string deviceOs, bool sandbox)
		{
			return CheckParametersAndAddAsync(deviceToken, cityId, deviceOs, sandbox);
		}

		public Task<IEnumerable<DeviceRecord>> GetDevicesAsync(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			return GetDevicesByCityAsync(cityAlias);
		}

		public Task<DeviceRecord> GetDeviceAsync(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

			return GetDeviceByTokenAsync(token);
		}

		public Task<bool> DoesExistAsync(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

			return DoseDeviceExistAsync(token);
		}

		public async Task<IEnumerable<DeviceRecord>> GetDevicesAsync()
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QueryAsync<DeviceRecord>(
					@"
SELECT
	D.DeviceToken AS DeviceToken,
	DOS.Os AS DeviceOs,
	D.Sandbox AS Sandbox,
	D.CityId AS CityId
FROM Devices D
INNER JOIN DeviceOs DOS
	ON DOS.Id = D.DeviceOsId");
			}
		}

		public async Task UpdateDeviceAsync(string oldToken, string newToken)
		{
			if (string.IsNullOrWhiteSpace(newToken) || string.IsNullOrWhiteSpace(oldToken))
				return;

			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"
UPDATE Devices
	SET DeviceToken = @newToken
WHERE DeviceToken = @oldToken",
					new
					{
						oldToken,
						newToken
					});
			}
		}

		public Task DeleteAsync(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

			return DeleteDeviceAsync(token);
		}

		private async Task<IEnumerable<DeviceRecord>> GetDevicesByCityAsync(string cityAlias)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QueryAsync<DeviceRecord>(
@"SELECT
	D.DeviceToken AS DeviceToken,
	DOS.Os AS DeviceOs,
	D.Sandbox AS Sandbox,
	D.CityId AS CityId
FROM Devices D
INNER JOIN DeviceOs DOS
	ON DOS.Id = D.DeviceOsId
INNER JOIN Cities C
	ON C.Id = D.CityId
WHERE C.Alias = @cityAlias",
					new { cityAlias});
			}
		}

		private async Task<DeviceRecord> GetDeviceByTokenAsync(string token)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleOrDefaultAsync<DeviceRecord>(
					@"
SELECT
	D.DeviceToken AS DeviceToken,
	DOS.Os AS DeviceOs,
	D.Sandbox AS Sandbox,
	D.CityId AS CityId
FROM Devices D
INNER JOIN DeviceOs DOS
	ON DOS.Id = D.DeviceOsId
WHERE D.DeviceToken = @token",
					new { token });
			}
		}

		private async Task DeleteDeviceAsync(string token)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
					@"
DELETE FROM Devices
WHERE DeviceToken = @token
",
					new { token });
			}
		}

		private Task CheckParametersAndAddAsync(string deviceToken, int? cityId, string deviceOs, bool sandbox)
		{
			if (string.IsNullOrWhiteSpace(deviceToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceToken));
			if (string.IsNullOrWhiteSpace(deviceOs))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceOs));

			return AddDeviceAsync(deviceToken, cityId, sandbox, deviceOs);
		}

		private async Task AddDeviceAsync(string deviceToken, int? cityId, bool sandbox, string deviceOs)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"INSERT OR REPLACE
INTO Devices (CityId, DeviceToken, Sandbox, DeviceOsId)
SELECT
	@cityId,
	@deviceToken,
	@sandbox,
	DOS.Id
FROM DeviceOs DOS
WHERE DOS.Os = @deviceOs",
					new
					{
						deviceToken,
						cityId,
						sandbox,
						deviceOs
					});
			}
		}

		private async Task<bool> DoseDeviceExistAsync(string token)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleAsync<bool>(
					@"SELECT EXISTS(SELECT 1 FROM Devices WHERE DeviceToken = @token)",
					new { token });
			}
		}
	}
}