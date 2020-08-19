using System;

namespace SadWave.Events.Api.Repositories.Events
{
	public class EventPhoto
	{
		public Uri EventUrl { get; set; }

		public Uri PhotoUrl { get; set; }

		public int PhotoWidth { get; set; }

		public int PhotoHeight { get; set; }
	}

	public class EventPhotoRecord
	{
		public string EventUrl { get; set; }

		public string PhotoUrl { get; set; }

		public int PhotoWidth { get; set; }

		public int PhotoHeight { get; set; }
	}
}
