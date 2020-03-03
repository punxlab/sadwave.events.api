using System;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookSettings
	{
		public FacebookSettings(string accessToken, string apiUrl, string apiVersion)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(accessToken));
			if (string.IsNullOrWhiteSpace(apiUrl))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiUrl));
			if (string.IsNullOrWhiteSpace(apiVersion))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiVersion));

			AccessToken = accessToken;
			ApiUrl = apiUrl;
			ApiVersion = apiVersion;
		}

		public string AccessToken { get; }

		public string ApiUrl { get; }

		public string ApiVersion { get; }
	}
}