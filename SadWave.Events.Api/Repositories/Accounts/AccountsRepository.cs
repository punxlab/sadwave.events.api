using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SadWave.Events.Api.Repositories.Accounts
{
	public class AccountsRepository : IAccountsRepository
	{
		private readonly IConnectionFactory<SqliteConnection> _connectionFactory;

		public AccountsRepository(IConnectionFactory<SqliteConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public Task CreateAsync(string login, byte[] hash, string role)
		{
			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
			if (string.IsNullOrWhiteSpace(role))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(role));
			if (hash == null)
				throw new ArgumentNullException(nameof(hash));
			if (hash.Length <= 0)
				throw new ArgumentException("Value length cannot be 0", nameof(hash));

			return CreateUserAsync(login, hash, role);
		}

		public Task<bool> DoesExistAsync(string login)
		{
			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));

			return DoesUserExistAsync(login);
		}

		public Task<AccountRecord> GetAsync(string login)
		{
			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));

			return GetUserAsync(login);
		}

		private async Task<AccountRecord> GetUserAsync(string login)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleOrDefaultAsync<AccountRecord>(
@"SELECT
	A.Login AS Login,
	A.Password AS Password,
	R.Name AS Role
FROM Accounts A
INNER JOIN Roles R
	ON R.Id = A.RoleId
WHERE A.Login = @login",
					new { login });
			}
		}

		private async Task CreateUserAsync(string login, byte[] hash, string role)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"INSERT INTO Accounts (Login, Password, RoleId)
SELECT
	@login,
	@hash,
	R.Id
FROM Roles R
WHERE R.Name = @role",
				new
				{
					login,
					hash,
					role
				});
			}
		}

		private async Task<bool> DoesUserExistAsync(string login)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				var result = await connection.ExecuteScalarAsync<int>(
@"SELECT
	COUNT(*)
FROM Accounts A
WHERE A.Login = @login",
					new
					{
						login
					});

				return result > 0;
			}
		}
	}
}