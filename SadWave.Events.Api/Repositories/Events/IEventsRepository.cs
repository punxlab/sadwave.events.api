using System.Collections.Generic;

namespace SadWave.Events.Api.Repositories.Events
{
	public interface IEventsRepository
	{
		IEnumerable<EventRecord> GetCityEvents(string cityAlias);

		void SetEvents(IEnumerable<EventRecord> events, string cityAlias);

		void RemoveEvents(string cityAlias);
	}
}