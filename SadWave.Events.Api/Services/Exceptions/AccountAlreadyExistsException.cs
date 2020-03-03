using System;
using System.Runtime.Serialization;

namespace SadWave.Events.Api.Services.Exceptions
{
	[Serializable]
	public class AccountAlreadyExistsException : Exception
	{
		public AccountAlreadyExistsException()
		{
		}

		public AccountAlreadyExistsException(string login) : base($"{login} account already exists.")
		{
		}

		public AccountAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected AccountAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}