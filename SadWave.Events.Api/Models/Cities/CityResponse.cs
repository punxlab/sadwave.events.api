using System;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Cities
{
	public class CityResponse
	{
		[JsonProperty("alias")]
		public string Alias { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("page")]
		public Uri Page { get; set; }
	}
}