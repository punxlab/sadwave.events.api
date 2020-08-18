using System;

namespace SadWave.Events.Api.Repositories.Events
{
	public class EventPhoto
	{
		public Uri EventUri { get; set; }

		public Uri PhotoUri { get; set; }

		public int PhotoWidth { get; set; }

		public int PhotoHeight { get; set; }
	}
}
