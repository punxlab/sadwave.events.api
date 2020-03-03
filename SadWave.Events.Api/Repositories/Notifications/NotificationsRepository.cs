using System;
using SadWave.Events.Api.Common.Storages;

namespace SadWave.Events.Api.Repositories.Notifications
{
	public class NotificationsRepository : INotificationsRepository
	{
		private readonly FileCacheStorage _storage;

		public NotificationsRepository(FileCacheStorage storage)
		{
			_storage = storage ?? throw new ArgumentNullException(nameof(storage));
		}

		public void SetDate(string cityAlias, DateTime date)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			var key = GetKey(cityAlias);
			_storage.Set(key, date);
		}

		public DateTime GetDate(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			var key = GetKey(cityAlias);
			return _storage.GetValue<DateTime>(key);
		}

		public void DeleteDate(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			var key = GetKey(cityAlias);
			_storage.Remove(key);
		}

		private static string GetKey(string alias)
		{
			return $"notification-date-{alias}";
		}
	}
}
