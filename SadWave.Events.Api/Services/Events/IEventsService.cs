using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Services.Events
{
	public interface IEventsService
	{
		Task<IEnumerable<Event>> GetCityEventsAsync(string cityAlias);

		Task SaveEventsAsync();

		Task DeleteEventsAsync();

		Task SetCustomEventPhotoAsync(Uri eventUrl, Uri photoUrl);
	}
}