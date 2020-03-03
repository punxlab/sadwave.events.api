using NUnit.Framework;
using SadWave.Events.Api.Common.Security;

namespace SadWave.Events.Api.Tests.Common.Security
{
	[TestFixture]
	public class PasswordEncryptorTests
	{
		[Test]
		public void VerifyReturnsTrueIfHashIsCorrect()
		{
			const string password = "TestPassword";
			var sut = new PasswordEncryptor();
			var hash = sut.Hash(password);
			Assert.IsTrue(sut.Verify(hash, password));
		}

		[Test]
		public void VerifyReturnsFalseIfHashIsIncorrect()
		{
			const string correctPassword = "TestPassword";
			const string incorrectPassword = "IncorrectTestPassword";
			var sut = new PasswordEncryptor();
			var hash = sut.Hash(correctPassword);
			Assert.IsFalse(sut.Verify(hash, incorrectPassword));
		}
	}
}
