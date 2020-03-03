using System;
using System.Collections.Generic;
using System.Linq;
using SadWave.Events.Api.Repositories.Events;

namespace SadWave.Events.Api.Services.Events
{
	public static class EventsUtils
	{
		public static bool HasNewEvents(IReadOnlyCollection<EventRecord> oldEvents, IReadOnlyCollection<EventRecord> newEvents)
		{
			if (newEvents == null || newEvents.Count == 0)
				return false;

			if (oldEvents == null || oldEvents.Count == 0)
				return true;

			var newActualEvents = newEvents
				.Where(IsActual)
				.ToList();

			var oldActualEvents = oldEvents
				.Where(IsActual)
				.ToList();

			if (newActualEvents.Count > oldActualEvents.Count)
				return true;

			return false;
		}

		private static bool IsActual(EventRecord record)
		{
			return record.Date.Date >= DateTime.UtcNow;
		}
	}
}
