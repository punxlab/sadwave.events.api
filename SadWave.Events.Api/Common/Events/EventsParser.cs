using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Events.Parsers;
using SadWave.Events.Api.Common.Events.Providers;
using SadWave.Events.Api.Common.Logger;
using VkNet.Exception;

namespace SadWave.Events.Api.Common.Events
{
	public class EventsParser : IEventsParser
	{
		private readonly ILogger _logger;
		private readonly EventDetailsProviderFactory _providerFactory;
		private readonly SadWaveEventsParser _eventsParser;

		public EventsParser(
			SadWaveEventsParser eventsParser,
			EventDetailsProviderFactory providerFactory,
			ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
			_eventsParser = eventsParser ?? throw new ArgumentNullException(nameof(eventsParser));
		}

		public Task<List<ParsedEventData>> ParseAsync(Uri eventsPageUri)
		{
			if (eventsPageUri == null) throw new ArgumentNullException(nameof(eventsPageUri));

			return ParseEventPageAsync(eventsPageUri);
		}

		private async Task<List<ParsedEventData>> ParseEventPageAsync(Uri eventsPageUri)
		{
			var sadWaveEvents = (await _eventsParser.ParseAsync(eventsPageUri)).Where(item => item.IsActual);

			var result = new List<ParsedEventData>();

			foreach (var sadWaveEvent in sadWaveEvents)
			{
				var resultedEvent = await CreateEventAsync(sadWaveEvent);
				if (resultedEvent != null)
				{
					result.Add(resultedEvent);
				}
			}

			return result
				.OrderBy(item => item.Date.Date)
				.ThenBy(item => item.Date.Time)
				.ToList();
		}

		private async Task<ParsedEventData> CreateEventAsync(SadWaveEvent sadWaveEvent)
		{
			try
			{
				var provider = _providerFactory.Create(sadWaveEvent.Url);

				if (provider == null)
				{
					_logger.Warning($"Provider for event {sadWaveEvent.Url} not found.");
					return CreateCommonEventData(sadWaveEvent);
				}

				var detailedEvent = await TryGetDataFromProviderAsync(provider, sadWaveEvent.Url);

				if(detailedEvent == null)
					return CreateCommonEventData(sadWaveEvent);

				TimeSpan? time = null;
				if (detailedEvent.StartDate.HasValue)
				{
					time = detailedEvent
						.StartDate
						.Value
						.ToUniversalTime()
						.TimeOfDay;
				}

				return new ParsedEventData
				{
					Name = detailedEvent.Name,
					Overview = sadWaveEvent.Text,
					Description = detailedEvent.Description,
					Date = new ParsedEventDate
					{
						Date = sadWaveEvent.Date,
						Time = time
					},
					Address = detailedEvent.Address,
					Url = sadWaveEvent.Url,
					ImageUrl = detailedEvent.ImageUrl,
					ImageHeight = detailedEvent.ImageHeight,
					ImageWidth = detailedEvent.ImageWidth,
				};
			}
			catch (Exception e)
			{
				_logger.Error(e);
				return null;
			}
		}

		private async Task<EventDetails> TryGetDataFromProviderAsync(IEventDetailsProvider provider, Uri uri)
		{
			try
			{
				return await provider.GetDetailsAsync(uri);
			}
			catch (UserAuthorizationFailException)
			{
				_logger.Warning($"Vk authorization failed for event {uri}.");
				return null;
			}
			catch (Exception e)
			{
				_logger.Error(e);
				return null;
			}
		}

		private static ParsedEventData CreateCommonEventData(SadWaveEvent sadWaveEvent)
		{
			return new ParsedEventData
			{
				Overview = sadWaveEvent.Text,
				Date = new ParsedEventDate
				{
					Date = sadWaveEvent.Date
				},
				Url = sadWaveEvent.Url
			};
		}
	}
}