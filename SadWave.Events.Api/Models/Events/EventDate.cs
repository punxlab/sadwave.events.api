using System;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Events
{
	public class EventDate
	{
		[JsonProperty("date")]
		public DateTime? Date { get; set; }

		[JsonProperty("time")]
		public TimeSpan? Time { get; set; }
	}
}