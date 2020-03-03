using System;

namespace SadWave.Events.Api.Repositories.Events
{
	[Serializable]
	public class EventRecord
	{
		public string Name { get; set; }

		public Uri Url { get; set; }

		public EventDateRecord Date { get; set; }

		public string Overview { get; set; }

		public string Description { get; set; }

		public string Address { get; set; }

		public Uri Photo { get; set; }

		public int PhotoWidth { get; set; }

		public int PhotoHeight { get; set; }
	}
}