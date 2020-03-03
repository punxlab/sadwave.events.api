using System;

namespace SadWave.Events.Api.Repositories.Events
{
	[Serializable]
	public class EventDateRecord
	{
		public DateTime? Date { get; set; }

		public TimeSpan? Time { get; set; }
	}
}