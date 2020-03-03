using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;

namespace SadWave.Events.Api.Common.Storages
{
	public class FileCacheStorage
	{
		private readonly FileCache _nativeStorage;

		public FileCacheStorage(FileCache nativeStorage)
		{
			_nativeStorage = nativeStorage ?? throw new ArgumentNullException(nameof(nativeStorage));
		}

		public void Set<T>(string key, T value)
		{
			SetValue(key, value);
		}

		public void Set<T>(string key, IEnumerable<T> values)
		{
			SetValue(key, values);
		}

		public IEnumerable<T> GetValues<T>(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

			return GetFromCache<IEnumerable<T>>(key, new List<T>());
		}

		public T GetValue<T>(string key)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

			return GetFromCache(key, default(T));
		}

		public void Remove(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

			_nativeStorage.Remove(key);
		}

		private void SetValue(string key, object value)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			_nativeStorage.Set(key, value, DateTimeOffset.MaxValue);
		}

		private T GetFromCache<T>(string key, T defaultValue)
		{
			try
			{
				var value = _nativeStorage.Get(key);
				if (value == null)
					return defaultValue;

				return (T)value;
			}
			catch (FileNotFoundException)
			{
				return defaultValue;
			}
		}
	}
}
