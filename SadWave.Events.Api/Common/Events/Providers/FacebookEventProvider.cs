using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Facebook;

namespace SadWave.Events.Api.Common.Events.Providers
{
	public class FacebookEventProvider: IEventDetailsProvider
	{
		private readonly FacebookClient _client;

		public const string HostName = "facebook.com";

		public FacebookEventProvider(FacebookClient client)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public Task<EventDetails> GetDetailsAsync(Uri eventUrl)
		{
			if (eventUrl == null)
				throw new ArgumentNullException(nameof(eventUrl));

			if (!eventUrl.Host.Contains(HostName))
				throw new ArgumentException($"Provided url ({eventUrl}) host should be equal {HostName}.");

			return GetDetailsByUrlAsync(eventUrl);
		}

		private async Task<EventDetails> GetDetailsByUrlAsync(Uri eventUrl)
		{
			var id = eventUrl.Segments.Last().Trim('/');
			var fbEvent = await _client.GetEventAsync(id);
			return new EventDetails
			{
				Name = fbEvent.Name,
				Description = fbEvent.Description,
				StartDate = fbEvent.Date,
				Address = GetAddress(fbEvent),
				ImageUrl = GetPicture(fbEvent)
			};
		}

		private static Uri GetPicture(FacebookEvent fbEvent)
		{
			return fbEvent.Picture?.Data == null ? null : new Uri(fbEvent.Picture.Data.Url);
		}

		private static string GetAddress(FacebookEvent fbEvent)
		{
			var streetBuilder = new StringBuilder();
			if (fbEvent.Place?.Location?.Street == null)
				return null;

			streetBuilder.Append(fbEvent.Place.Location.Street);
			if (!string.IsNullOrWhiteSpace(fbEvent.Place.Name))
			{
				streetBuilder.Append($", {fbEvent.Place.Name}");
			}

			return streetBuilder.ToString();
		}
	}
}
