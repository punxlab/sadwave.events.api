using System;
using System.Runtime.Serialization;

namespace SadWave.Events.Api.Services.Exceptions
{
	[Serializable]
	public class CityNotFoundException : Exception
	{
		public CityNotFoundException()
		{
		}

		public CityNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected CityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}