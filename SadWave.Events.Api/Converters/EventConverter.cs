using System;
using SadWave.Events.Api.Common.Events;
using SadWave.Events.Api.Models.Events;
using SadWave.Events.Api.Repositories.Events;
using SadWave.Events.Api.Services.Events;

namespace SadWave.Events.Api.Converters
{
	public static class EventConverter
	{
		public static Event Convert(EventRecord record)
		{
			if (record == null)
				throw new ArgumentNullException(nameof(record));

			return new Event
			{
				Date = record.Date?.Date,
				Time = record.Date?.Time,
				Url = record.Url,
				Name = record.Name,
				Overview = record.Overview,
				Photo = record.Photo,
				PhotoHeight = record.PhotoHeight,
				PhotoWidth = record.PhotoWidth
			};
		}

		public static EventRecord Convert(ParsedEventData eventData)
		{
			if (eventData == null)
				throw new ArgumentNullException(nameof(eventData));

			return new EventRecord
			{
				Name = eventData.Name,
				Date = new EventDateRecord { Date = eventData.Date.Date, Time = eventData.Date.Time },
				Overview = eventData.Overview,
				Photo = eventData.ImageUrl,
				PhotoHeight = eventData.ImageHeight,
				PhotoWidth = eventData.ImageWidth,
				Url = eventData.Url
			};
		}

		public static EventResponse Convert(Event record)
		{
			if (record == null)
				throw new ArgumentNullException(nameof(record));

			return new EventResponse
			{
				Date = new EventDate { Date = record.Date, Time = record.Time },
				Url = record.Url,
				Name = record.Name,
				Overview = record.Overview,
				Photo = record.Photo,
				PhotoHeight = record.PhotoHeight,
				PhotoWidth = record.PhotoWidth
			};
		}
	}
}
