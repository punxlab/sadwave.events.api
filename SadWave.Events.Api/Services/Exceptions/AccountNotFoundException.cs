using System;
using System.Runtime.Serialization;

namespace SadWave.Events.Api.Services.Exceptions
{
	[Serializable]
	public class AccountNotFoundException : Exception
	{
		public AccountNotFoundException()
		{
		}

		public AccountNotFoundException(string login) : base ($"{login} account not found.")
		{
		}

		public AccountNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected AccountNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}