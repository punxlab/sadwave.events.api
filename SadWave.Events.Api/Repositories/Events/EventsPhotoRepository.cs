using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories.Events
{
	public class EventsPhotoRepository : IEventsPhotoRepository
	{
		private readonly IConnectionFactory<SqliteConnection> _connectionFactory;

		public EventsPhotoRepository(IConnectionFactory<SqliteConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public Task RemoveEventPhotoAsync(Uri eventUrl)
		{
			if (eventUrl is null)
				throw new ArgumentNullException(nameof(eventUrl));

			return DeleteEventPhotoAsync(eventUrl.ToString());
		}

		public Task<EventPhoto> GetEventPhotoAsync(Uri eventUrl)
		{
			if (eventUrl is null)
				throw new ArgumentNullException(nameof(eventUrl));

			return SelectEventPhotoAsync(eventUrl.ToString());
		}

		public Task SetPhotoAsync(EventPhoto photo)
		{
			if (photo is null)
				throw new ArgumentNullException(nameof(photo));
			if (photo.EventUrl is null)
				throw new ArgumentNullException(nameof(photo.EventUrl));

			return AddEventPhotoAsync(
				photo.EventUrl.ToString(),
				photo.PhotoUrl?.ToString(),
				photo.PhotoWidth,
				photo.PhotoHeight
			);
		}

		private async Task<EventPhoto> SelectEventPhotoAsync(string eventUrl)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				var record = await connection.QuerySingleOrDefaultAsync<EventPhotoRecord>(
					@"
SELECT
	E.EventUrl AS EventUrl,
	E.PhotoUrl AS PhotoUrl,
	E.PhotoWidth AS PhotoWidth,
	E.PhotoHeight AS PhotoHeight
FROM EventsPhotos E
WHERE E.EventUrl = @eventUrl",
					new
					{
						eventUrl
					}
				);

				if (record == null)
					return null;

				return new EventPhoto
				{
					EventUrl = new Uri(record.EventUrl),
					PhotoUrl = record.PhotoUrl == null ? null : new Uri(record.PhotoUrl),
					PhotoWidth = record.PhotoWidth,
					PhotoHeight = record.PhotoHeight
				};
			}
		}

		private async Task AddEventPhotoAsync(string eventUrl, string photoUrl, int photoWidth, int photoHeight)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"INSERT OR REPLACE
INTO EventsPhotos (EventUrl, PhotoUrl, PhotoWidth, PhotoHeight)
VALUES (@eventUrl, @photoUrl, @photoWidth, @photoHeight)",
					new
					{
						eventUrl,
						photoUrl,
						photoWidth,
						photoHeight
					});
			}
		}

		private async Task DeleteEventPhotoAsync(string eventUrl)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				await connection.ExecuteAsync(
@"DELETE FROM EventsPhotos
WHERE EventUrl = @eventUrl",
					new
					{
						eventUrl,
					});
			}
		}
	}
}
