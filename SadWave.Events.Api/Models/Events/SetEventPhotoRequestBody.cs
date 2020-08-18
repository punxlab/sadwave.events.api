using Newtonsoft.Json;
using System;

namespace SadWave.Events.Api.Models.Events
{
	public class SetEventPhotoRequestBody
	{
		[JsonProperty("event")]
		public Uri EventUrl { get; set; }

		[JsonProperty("photo")]
		public Uri PhotoUri { get; set; }
	}
}
