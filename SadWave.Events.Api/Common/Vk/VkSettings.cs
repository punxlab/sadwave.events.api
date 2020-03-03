using System;

namespace SadWave.Events.Api.Common.Vk
{
	public class VkSettings
	{
		public string AccessToken { get; }

		public VkSettings(string accessToken)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(accessToken));
			AccessToken = accessToken;
		}
	}
}