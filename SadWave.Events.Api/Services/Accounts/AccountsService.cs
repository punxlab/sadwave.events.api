using System;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Security;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Repositories.Accounts;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Services.Accounts
{
	public class AccountsService : IAccountsService
	{
		private readonly IAccountsRepository _repository;
		private readonly IPasswordEncryptor _encryptor;

		public AccountsService(IAccountsRepository repository, IPasswordEncryptor encryptor)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
		}

		public Task CreateAsync(string login, string password, Role role)
		{
			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
			if (!RoleUtils.TryGetName(role, out var roleName))
				throw new ArgumentException($"Unexpected role value: { role }.");

			return CreateAccountAsync(login, password, roleName);
		}

		public Task<Account> GetAsync(string login, string password)
		{
			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

			return GetAccountAsync(login, password);
		}

		private async Task<Account> GetAccountAsync(string login, string password)
		{
			var accountRecord = await _repository.GetAsync(login);
			if(accountRecord == null)
				throw new AccountNotFoundException(login);

			if (!_encryptor.Verify(accountRecord.Password, password))
				throw new IncorrectPasswordException(login);

			return AccountConverter.Convert(accountRecord);
		}

		private async Task CreateAccountAsync(string login, string password, string role)
		{
			if (await _repository.DoesExistAsync(login))
				throw new AccountAlreadyExistsException(login);

			var hash = _encryptor.Hash(password);

			await _repository.CreateAsync(login, hash, role);
		}
	}
}
