using System;
using System.Runtime.Serialization;

namespace SadWave.Events.Api.Services.Exceptions
{
	[Serializable]
	public class UnknownOsException : Exception
	{
		public UnknownOsException()
		{
		}

		public UnknownOsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UnknownOsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}