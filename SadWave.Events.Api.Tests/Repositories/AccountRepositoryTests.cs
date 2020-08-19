using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using SadWave.Events.Api.Repositories.Accounts;
using SadWave.Events.Api.Services.Accounts;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class AccountRepositoryTests : BaseDatabaseRepositoryTest
	{
		public AccountRepositoryTests() : base("account-test.sqlite")
		{
		}

		[Test]
		public async Task CreateAccountTest()
		{
			const string expectedLogin = "test-login";
			const string expectedRole = RoleName.User;
			var expectedPasswordHash = Encoding.ASCII.GetBytes("test-password-hash");

			var sut = CreateSut();

			await sut.CreateAsync(expectedLogin, expectedPasswordHash, expectedRole);

			var actual = await sut.GetAsync(expectedLogin);

			actual.Should().NotBeNull();
			actual.Login.Should().NotBeNull();
			actual.Login.Should().Be(expectedLogin);
			actual.Password.Should().NotBeNull();
			actual.Password.Should().BeEquivalentTo(expectedPasswordHash);
			actual.Role.Should().Be(expectedRole);
		}

		[Test]
		public async Task CreateDoesntCreateAccountIfRoleDoesNotExistTest()
		{
			const string expectedLogin = "test-login";
			const string incorrectRole = "IncorrectRole";
			var expectedPasswordHash = Encoding.ASCII.GetBytes("test-password-hash");

			var sut = CreateSut();

			await sut.CreateAsync(expectedLogin, expectedPasswordHash, incorrectRole);
			var actual = await sut.GetAsync(expectedLogin);
			actual.Should().BeNull();
		}

		[Test]
		public async Task CreateAccountDoesntCreateDuplicatesTest()
		{
			const string expectedLogin = "test-login";
			const string expectedRole = RoleName.User;
			var expectedPasswordHash = Encoding.ASCII.GetBytes("test-password-hash");

			var sut = CreateSut();

			await sut.CreateAsync(expectedLogin, expectedPasswordHash, expectedRole);

			var actual = Assert.ThrowsAsync<SqliteException>(
				async () => await sut.CreateAsync(expectedLogin, expectedPasswordHash, expectedRole));
			actual.Message.Should().Contain("UNIQUE constraint failed: Accounts.Login");
		}

		[Test]
		public async Task GetReturnsNullIfAccountDoesNotExistTest()
		{
			const string unknownLogin = "test-login";
			var sut = CreateSut();

			var actual = await sut.GetAsync(unknownLogin);
			actual.Should().BeNull();
		}

		[Test]
		public async Task DoesExistReturnsTrueIfAccountExistsTest()
		{
			const string expectedLogin = "test-login";
			const string expectedRole = RoleName.User;
			var expectedPasswordHash = Encoding.ASCII.GetBytes("test-password-hash");

			var sut = CreateSut();

			await sut.CreateAsync(expectedLogin, expectedPasswordHash, expectedRole);

			var actual = await sut.DoesExistAsync(expectedLogin);
			actual.Should().BeTrue();
		}

		[Test]
		public async Task DoesExistReturnsFalseIfAccountDoesNotExistTest()
		{
			const string incorrectLogin = "test-login";

			var sut = CreateSut();

			var actual = await sut.DoesExistAsync(incorrectLogin);
			actual.Should().BeFalse();
		}

		private AccountsRepository CreateSut()
		{
			return new AccountsRepository(ConnectionFactory);
		}
	}
}
