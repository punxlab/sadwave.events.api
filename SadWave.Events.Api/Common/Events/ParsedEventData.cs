using System;

namespace SadWave.Events.Api.Common.Events
{
	public class ParsedEventData
	{
		public string Name { get; set; }

		public Uri Url { get; set; }

		public ParsedEventDate Date { get; set; }

		public string Overview { get; set; }

		public string Description { get; set; }

		public string Address { get; set; }

		public Uri ImageUrl { get; set; }

		public int ImageHeight { get; set; }

		public int ImageWidth { get; set; }
	}
}