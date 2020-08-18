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

			return DeleteEventPhotoAsync(eventUrl);
		}

		public Task<EventPhoto> GetEventPhotoAsync(Uri eventUrl)
		{
			if (eventUrl is null)
				throw new ArgumentNullException(nameof(eventUrl));

			return SelectEventPhotoAsync(eventUrl);
		}

		public Task SetPhotoAsync(EventPhoto photo)
		{
			if (photo is null)
				throw new ArgumentNullException(nameof(photo));
			if (photo.EventUri is null)
				throw new ArgumentNullException(nameof(photo.EventUri));
			if (photo.PhotoUri is null)
				throw new ArgumentNullException(nameof(photo.PhotoUri));

			return AddEventPhotoAsync(photo.EventUri, photo.PhotoUri, photo.PhotoWidth, photo.PhotoHeight);
		}

		public async Task<EventPhoto> SelectEventPhotoAsync(Uri eventUrl)
		{
			using (var connection = await _connectionFactory.CreateConnectionAsync())
			{
				return await connection.QuerySingleOrDefaultAsync<EventPhoto>(
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
			}
		}

		private async Task AddEventPhotoAsync(Uri eventUrl, Uri photoUrl, int photoWidth, int photoHeight)
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

		private async Task DeleteEventPhotoAsync(Uri eventUrl)
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
