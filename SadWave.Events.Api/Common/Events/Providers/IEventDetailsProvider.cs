using System;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Common.Events.Providers
{
	public interface IEventDetailsProvider
	{
		Task<EventDetails> GetDetailsAsync(Uri eventUrl);
	}
}
