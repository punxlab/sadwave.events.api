using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Events;
using SadWave.Events.Api.Common.Images;
using SadWave.Events.Api.Common.Logger;
using SadWave.Events.Api.Common.Notifications;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Messages;
using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Repositories.Devices;
using SadWave.Events.Api.Repositories.Events;
using SadWave.Events.Api.Services.Exceptions;
using SadWave.Events.Api.Services.Notifications;

namespace SadWave.Events.Api.Services.Events
{
	public class EventsService : IEventsService
	{
		private readonly IEventsRepository _eventsRepository;
		private readonly IEventsPhotoRepository _eventsPhotoRepository;
		private readonly ICitiesRepository _citiesRepository;
		private readonly IEventsParser _eventsParser;
		private readonly IDevicesRepository _devicesRepository;
		private readonly INotificationsService _notificationsService;
		private readonly IImageSizeProvider _imageSizeProvider;
		private readonly ILogger _logger;

		public EventsService(
			IEventsRepository eventsRepository,
			ICitiesRepository citiesRepository,
			IEventsPhotoRepository eventsPhotoRepository,
			IDevicesRepository devicesRepository,
			IEventsParser eventsParser,
			INotificationsService notificationsService,
			IImageSizeProvider imageSizeProvider,
			ILogger logger)
		{
			_eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
			_eventsPhotoRepository = eventsPhotoRepository ?? throw new ArgumentNullException(nameof(eventsPhotoRepository));
			_citiesRepository = citiesRepository ?? throw new ArgumentNullException(nameof(citiesRepository));
			_eventsParser = eventsParser ?? throw new ArgumentNullException(nameof(eventsParser));
			_devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
			_notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
			_imageSizeProvider = imageSizeProvider ?? throw new ArgumentNullException(nameof(imageSizeProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IEnumerable<Event>> GetCityEventsAsync(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			var events = await GetEventsByCityAsync(cityAlias);
			return events.Select(EventConverter.Convert);
		}

		public async Task SaveEventsAsync()
		{
			var cities = await _citiesRepository.GetAsync();

			foreach (var city in cities)
			{
				try
				{
					var storedEvents = _eventsRepository.GetCityEvents(city.Alias).ToList();
					var parsedEvents = await _eventsParser.ParseAsync(new Uri(city.PageUrl));
					var newEvents = parsedEvents.Select(EventConverter.Convert).ToList();

					_eventsRepository.SetEvents(newEvents, city.Alias);

					if (!EventsUtils.HasNewEvents(storedEvents, newEvents))
					{
						_logger.Information($"New events not found for {city.Alias}.");
						continue;
					}

					await RemoveObsoleteCustomEventPhotoAsync(storedEvents, parsedEvents);

					_logger.Information($"Has new events for {city.Alias}.");
					var devicesRecords = await _devicesRepository.GetDevicesAsync(city.Alias);

					var notification = new Notification
					{
						Devices = devicesRecords.Select(DeviceConverter.Convert),
						Message = MessagesFactory.CreateNewEventsAlert(city.Name),
						CityAlias = city.Alias
					};

					Notify(notification);
				}
				catch (Exception e)
				{
					_logger.Error($"Unable to save events for city {city.Alias}", e);
				}
			}
		}

		private async Task RemoveObsoleteCustomEventPhotoAsync(List<EventRecord> storedEvents, List<ParsedEventData> parsedEvents)
		{
			foreach (var storedEvent in storedEvents)
			{

				if (!parsedEvents.Any(e => e.Url == storedEvent.Url))
				{
					try
					{
						await _eventsPhotoRepository.RemoveEventPhotoAsync(storedEvent.Url);
					}
					catch(Exception e)
					{
						_logger.Error(e);
					}
				}
			}
		}

		public async Task SetCustomEventPhotoAsync(Uri eventUrl, Uri photoUrl)
		{
			if (eventUrl is null)
				throw new ArgumentNullException(nameof(eventUrl));

			var cities = await _citiesRepository.GetAsync();
			foreach (var city in cities)
			{
				var events = _eventsRepository.GetCityEvents(city.Alias);
				var eventValue = events.SingleOrDefault(e => e.Url == eventUrl);
				if (eventValue != null)
				{
					ImageSize size = new ImageSize();
					if (photoUrl != null) {
						size = await _imageSizeProvider.GetSizeByUriAsync(photoUrl);
					}

					await _eventsPhotoRepository.SetPhotoAsync(
						new EventPhoto {
							EventUrl = eventValue.Url,
							PhotoUrl = eventValue.Photo,
							PhotoWidth = size.Width,
							PhotoHeight = size.Height
						}
					);

					break;
				}
			}
		}

		public async Task DeleteEventsAsync()
		{
			var cities = await _citiesRepository.GetAsync();
			foreach (var city in cities)
			{
				_eventsRepository.RemoveEvents(city.Alias);
			}
		}

		private async Task<IEnumerable<EventRecord>> GetEventsByCityAsync(string cityAlias)
		{
			var cityRecord = await _citiesRepository.GetAsync(cityAlias);
			if (cityRecord == null)
				throw new CityNotFoundException();

			return _eventsRepository.GetCityEvents(cityAlias);
		}

		private void Notify(Notification notification)
		{
			if (_notificationsService.WasCityNotifiedToday(notification.CityAlias))
			{
				_logger.Information($"A notification for {notification.CityAlias} will not be send. Already had a notification for today.");
				return;
			}

			_logger.Information($"A notification for {notification.CityAlias} will be send");
			_notificationsService.Notify(notification);
			_notificationsService.MarkCityAsNotified(notification.CityAlias);
		}
	}
}