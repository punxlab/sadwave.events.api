using System;
using System.Runtime.Serialization;

namespace SadWave.Events.Api.Services.Exceptions
{
	[Serializable]
	public class IncorrectPasswordException : Exception
	{
		public IncorrectPasswordException()
		{
		}

		public IncorrectPasswordException(string login) : base($"Incorrect password for {login}.")
		{
		}

		public IncorrectPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected IncorrectPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}