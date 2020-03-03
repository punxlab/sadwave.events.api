using System;
using JetBrains.Annotations;

namespace SadWave.Events.Api.Messages
{
	public static class MessagesFactory
	{
		public static string CreateNewEventsAlert(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(city));
			return $"{city}. Новые гиги, мертвецы!";
		}
	}
}