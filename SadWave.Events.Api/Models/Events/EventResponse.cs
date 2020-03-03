using System;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Events
{
	public class EventResponse
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("url")]
		public Uri Url { get; set; }

		[JsonProperty("date")]
		public EventDate Date { get; set; }

		[JsonProperty("overview")]
		public string Overview { get; set; }

		[JsonProperty("photo")]
		public Uri Photo { get; set; }

		[JsonProperty("photoWidth")]
		public int PhotoWidth { get; set; }

		[JsonProperty("photoHeight")]
		public int PhotoHeight { get; set; }
	}
}