using System;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookEvent
	{
		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("start_time")]
		public DateTime Date { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("place")]
		public FacebookPlace Place { get; set; }

		[JsonProperty("picture")]
		public FacebookPicture Picture { get; set; }
	}
}