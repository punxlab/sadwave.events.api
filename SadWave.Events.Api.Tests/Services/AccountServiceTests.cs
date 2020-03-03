using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SadWave.Events.Api.Common.Security;
using SadWave.Events.Api.Repositories.Accounts;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Tests.Services
{
	[TestFixture]
	public class AccountServiceTests
	{
		[Test]
		public void CreateAccountTest()
		{
			const string expectedLogin = "test-login";
			const string expectedPassword = "test-password";
			const Role expectedRole = Role.User;
			const string expectedRoleName = RoleName.User;

			var expectedPasswordHash = Encoding.ASCII.GetBytes("incorrect-test-password-hash");

			RunTest(async (sut, repositoryMock, encryptorMock) =>
			{
				repositoryMock
					.Setup(repository => repository.DoesExistAsync(expectedLogin))
					.ReturnsAsync(false);

				encryptorMock
					.Setup(encryptor => encryptor.Hash(expectedPassword))
					.Returns(expectedPasswordHash);

				repositoryMock
					.Setup(repository => repository.CreateAsync(expectedLogin, expectedPasswordHash, expectedRoleName))
					.Returns(Task.CompletedTask);

				await sut.CreateAsync(expectedLogin, expectedPassword, expectedRole);
			});
		}

		[Test]
		public void CreateAccountThrowsExceptionIfRoleDoesNotExitsTest()
		{
			const string expectedLogin = "test-login";
			const string expectedPassword = "test-password";
			const Role incorrectRole = (Role)int.MaxValue;

			RunTest((sut, repositoryMock, encryptorMock) =>
			{
				var actual = Assert.ThrowsAsync<ArgumentException>(
					async () => await sut.CreateAsync(expectedLogin, expectedPassword, incorrectRole));
				actual.Message.Should().Be($"Unexpected role value: {incorrectRole}.");
			});
		}

		[Test]
		public void CreateAccountThrowsExceptionIfItAlreadyExistsTest()
		{
			const string expectedLogin = "test-login";
			const string expectedPassword = "test-password";
			const Role expectedRole = Role.User;

			RunTest((sut, repositoryMock, encryptorMock) =>
			{
				repositoryMock
					.Setup(repository => repository.DoesExistAsync(expectedLogin))
					.ReturnsAsync(true);

				var actual = Assert.ThrowsAsync<AccountAlreadyExistsException>(
					async () => await sut.CreateAsync(expectedLogin, expectedPassword, expectedRole));
				actual.Message.Should().Be($"{expectedLogin} account already exists.");
			});
		}

		[Test]
		public void GetAccountThrowsExceptionIfAccountDoesNotExistTest()
		{
			const string unknownLogin = "test-login";
			const string unknownPassword = "test-password";

			RunTest((sut, repositoryMock, encryptorMock) =>
			{
				repositoryMock
					.Setup(repository => repository.GetAsync(unknownLogin))
					.ReturnsAsync((AccountRecord) null);

				var actual = Assert.ThrowsAsync<AccountNotFoundException>(
					async () => await sut.GetAsync(unknownLogin, unknownPassword));
				actual.Message.Should().Be($"{unknownLogin} account not found.");
			});
		}

		[Test]
		public void GetAccountThrowsExceptionIfPasswordIsIncorrectTest()
		{
			const string expectedLogin = "test-login";
			const string incorrectPassword = "incorrect-test-password";
			var incorrectPasswordHash = Encoding.ASCII.GetBytes("incorrect-test-password-hash");

			var expectedAccountRecord  = new AccountRecord
			{
				Login = expectedLogin,
				Password = incorrectPasswordHash,
				Role = RoleName.User
			};

			RunTest((sut, repositoryMock, encryptorMock) =>
			{
				repositoryMock
					.Setup(repository => repository.GetAsync(expectedLogin))
					.ReturnsAsync(expectedAccountRecord);

				encryptorMock
					.Setup(encryptor => encryptor.Verify(incorrectPasswordHash, incorrectPassword))
					.Returns(false);

				Assert.ThrowsAsync<IncorrectPasswordException>(async () => await sut.GetAsync(expectedLogin, incorrectPassword));
			});
		}

		private static void RunTest(Action<AccountsService, Mock<IAccountsRepository>, Mock<IPasswordEncryptor>> action)
		{

			var repositoryMock = new Mock<IAccountsRepository>();
			var encryptorMock = new Mock<IPasswordEncryptor>();

			var sut = new AccountsService(repositoryMock.Object, encryptorMock.Object);
			action(sut, repositoryMock, encryptorMock);
			repositoryMock.VerifyAll();
			encryptorMock.VerifyAll();
		}
	}
}
