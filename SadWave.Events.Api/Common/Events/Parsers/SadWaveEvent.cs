using System;

namespace SadWave.Events.Api.Common.Events.Parsers
{
	public class SadWaveEvent
	{
		public string Text { get; set; }

		public DateTime Date { get; set; }

		public Uri Url { get; set; }

		public bool IsActual => Date >= DateTime.UtcNow.Date;
	}
}