using System;
using System.Collections.Generic;
using SadWave.Events.Api.Common.Storages;

namespace SadWave.Events.Api.Repositories.Events
{
	public class EventsRepository : IEventsRepository
	{
		private readonly FileCacheStorage _storage;

		public EventsRepository(FileCacheStorage storage)
		{
			_storage = storage ?? throw new ArgumentNullException(nameof(storage));
		}

		public IEnumerable<EventRecord> GetCityEvents(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			return _storage.GetValues<EventRecord>(GetKey(cityAlias));
		}

		public void SetEvents(IEnumerable<EventRecord> events, string cityAlias)
		{
			if (events == null) throw new ArgumentNullException(nameof(events));

			if (string.IsNullOrEmpty(cityAlias))
				throw new ArgumentException("Value cannot be null or empty.", nameof(cityAlias));

			_storage.Set(GetKey(cityAlias), events);
		}

		public void RemoveEvents(string cityAlias)
		{
			if (string.IsNullOrEmpty(cityAlias))
				throw new ArgumentException("Value cannot be null or empty.", nameof(cityAlias));

			_storage.Remove(GetKey(cityAlias));
		}

		private static string GetKey(string alias)
		{
			return $"events-{alias}";
		}
	}
}