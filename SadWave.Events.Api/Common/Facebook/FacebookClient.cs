using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookClient
	{
		private readonly FacebookSettings _settings;

		public FacebookClient(FacebookSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public Task<FacebookEvent> GetEventAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentException("message", nameof(id));

			return GetEventByIdAsync(id);
		}

		private async Task<FacebookEvent> GetEventByIdAsync(string id)
		{
			return await GetAsync<FacebookEvent>(
				id,
				new Dictionary<string, string>
				{
					{
						"fields",
						"id,name,description,place,start_time,picture.type(large)"
					}
				});
		}

		private async Task<T> GetAsync<T>(string path, Dictionary<string, string> parameters = null)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUri = $"{_settings.ApiUrl}/v{_settings.ApiVersion}/{path}";
				if (parameters != null)
				{
					foreach (var parameter in parameters)
					{
						requestUri = $"{requestUri}?{parameter.Key}={parameter.Value}&access_token={_settings.AccessToken}";
					}
				}
				else
				{
					requestUri = $"{requestUri}?access_token={_settings.AccessToken}";
				}

				httpClient.BaseAddress = new Uri(requestUri);
				var response = await httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync();

				var resultedObject = JObject.Parse(response);
				if (resultedObject["error"] != null)
				{
					var error = resultedObject["error"].ToObject<FacebookErrorResponse>();
					throw new FacebookApiException(error.Message);
				}

				return JsonConvert.DeserializeObject<T>(response);
			}
		}
	}
}