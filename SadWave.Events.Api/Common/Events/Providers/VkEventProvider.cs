using System;
using System.Linq;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Vk;

namespace SadWave.Events.Api.Common.Events.Providers
{
	public class VkEventProvider : IEventDetailsProvider
	{
		private const string EventKindGroup = "event";

		private readonly VkClient _client;

		public const string HostName = "vk.com";

		public VkEventProvider(VkClient client)
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

		public async Task<EventDetails> GetDetailsByUrlAsync(Uri eventUrl)
		{
			var latsUrlSegment = eventUrl.Segments.Last();
			var eventId = latsUrlSegment.StartsWith(EventKindGroup) ?
				latsUrlSegment.Replace(EventKindGroup, string.Empty) :
				latsUrlSegment;

			var vkEvent = await _client.GetEventAsync(eventId);

			var imageUrl = vkEvent.Photo200;
			var imageHeight = 200;
			var imageWidth = 200;

			if (vkEvent.MainAlbumId != null)
			{
				var vkAlbumPhotos = await _client.GetPhotoAsync(vkEvent.MainAlbumId.Value, -vkEvent.Id);
				
				if (vkAlbumPhotos != null && vkAlbumPhotos.Count > 0)
				{
					var firstAlbumPhoto = vkAlbumPhotos.First();
					var biggestPhoto = firstAlbumPhoto?.GetTheBiggest();

					if (biggestPhoto != null)
					{
						imageUrl = biggestPhoto.Url;
						imageHeight = Convert.ToInt32(biggestPhoto.Height);
						imageWidth = Convert.ToInt32(biggestPhoto.Width);
					}
				}
			}

			return new EventDetails
			{
				Name = vkEvent.Name,
				Description = vkEvent.Description,
				StartDate = vkEvent.StartDate,
				ImageUrl = imageUrl,
				ImageHeight = imageHeight,
				ImageWidth = imageWidth,
				Address = vkEvent.Place?.Address
			};
		}
	}
}