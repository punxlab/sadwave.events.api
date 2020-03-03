using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SadWave.Events.Api.Common.Events;
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
		private readonly ICitiesRepository _citiesRepository;
		private readonly IEventsParser _eventsParser;
		private readonly IDevicesRepository _devicesRepository;
		private readonly INotificationsService _notificationsService;
		private readonly ILogger _logger;

		public EventsService(
			IEventsRepository eventsRepository,
			ICitiesRepository citiesRepository,
			IEventsParser eventsParser,
			IDevicesRepository devicesRepository,
			INotificationsService notificationsService,
			ILogger logger)
		{
			_eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
			_citiesRepository = citiesRepository ?? throw new ArgumentNullException(nameof(citiesRepository));
			_eventsParser = eventsParser ?? throw new ArgumentNullException(nameof(eventsParser));
			_devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
			_notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public Task<IEnumerable<Event>> GetCityEventsAsync(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			return GetEventsByCityAsync(cityAlias);
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

		public async Task DeleteEventsAsync()
		{
			var cities = await _citiesRepository.GetAsync();
			foreach (var city in cities)
			{
				_eventsRepository.RemoveEvents(city.Alias);
			}
		}

		private async Task<IEnumerable<Event>> GetEventsByCityAsync(string cityAlias)
		{
			var cityRecord = await _citiesRepository.GetAsync(cityAlias);
			if (cityRecord == null)
				throw new CityNotFoundException();

			var events = _eventsRepository.GetCityEvents(cityAlias);

			return events.Select(EventConverter.Convert);
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