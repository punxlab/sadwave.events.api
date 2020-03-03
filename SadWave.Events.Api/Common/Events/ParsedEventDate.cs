using System;

namespace SadWave.Events.Api.Common.Events
{
	public class ParsedEventDate
	{
		public DateTime? Date { get; set; }

		public TimeSpan? Time { get; set; }
	}
}