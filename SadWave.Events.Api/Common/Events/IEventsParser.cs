using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Common.Events
{
	public interface IEventsParser
	{
		Task<List<ParsedEventData>> ParseAsync(Uri eventsPageUri);
	}
}