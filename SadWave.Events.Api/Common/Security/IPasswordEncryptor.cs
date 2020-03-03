namespace SadWave.Events.Api.Common.Security
{
	public interface IPasswordEncryptor
	{
		byte[] Hash(string password);

		bool Verify(byte[] hashedPassword, string verifingPassword);
	}
}