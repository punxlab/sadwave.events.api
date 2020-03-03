using System;

namespace SadWave.Events.Api.Services.Events
{
	public class Event
	{
		public string Name { get; set; }

		public Uri Url { get; set; }

		public DateTime? Date { get; set; }

		public TimeSpan? Time { get; set; }

		public string Overview { get; set; }

		public Uri Photo { get; set; }

		public int PhotoWidth { get; set; }

		public int PhotoHeight { get; set; }
	}
}