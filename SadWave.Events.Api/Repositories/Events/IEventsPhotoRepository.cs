using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories.Events
{
	public interface IEventsPhotoRepository
	{
		Task SetPhotoAsync(EventPhoto photo);

		Task<EventPhoto> GetEventPhotoAsync(Uri eventUri);

		Task RemoveEventPhotoAsync(Uri eventUrl);
	}
}
