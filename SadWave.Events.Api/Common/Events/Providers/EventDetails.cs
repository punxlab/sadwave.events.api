using System;

namespace SadWave.Events.Api.Common.Events.Providers
{
	public class EventDetails
	{
		public string Name { get; set; }

		public DateTime? StartDate { get; set; }

		public string Address { get; set; }

		public string Description { get; set; }

		public Uri ImageUrl { get; set; }

		public int ImageHeight { get; set; }

		public int ImageWidth { get; set; }
	}
}