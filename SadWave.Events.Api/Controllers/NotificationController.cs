using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SadWave.Events.Api.Common.Notifications;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Models.Notification;
using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Repositories.Devices;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Notifications;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/notification")]
	public class NotificationController : Controller
	{
		private readonly ICitiesRepository _citiesRepository;
		private readonly IDevicesRepository _devicesRepository;
		private readonly INotificationsService _notificationsService;

		public NotificationController(
			ICitiesRepository citiesRepository,
			IDevicesRepository devicesRepository,
			INotificationsService notificationsService)
		{
			_citiesRepository = citiesRepository ?? throw new ArgumentNullException(nameof(citiesRepository));
			_devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
			_notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
		}

		[HttpPost]
		[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> NotifyAsync([FromBody]NotificationRequestBody body)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			if (string.IsNullOrWhiteSpace(body.DeviceToken) && string.IsNullOrWhiteSpace(body.Os))
				return BadRequest();

			if (!string.IsNullOrWhiteSpace(body.DeviceToken) && !string.IsNullOrWhiteSpace(body.Os))
				return BadRequest();

			if (!string.IsNullOrWhiteSpace(body.DeviceToken))
				return await NotifyByDeviceAsync(body);

			return await NotifyByOsAsync(body);
		}

		[HttpDelete("date")]
		[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> DeleteNotificationDateAsync(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				return BadRequest();

			if (!await _citiesRepository.DoesExistAsync(city))
				return NotFound();

			_notificationsService.DeleteLastNotificationDate(city);

			return Ok();
		}

		[HttpGet("date")]
		[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> GetNotificationDateAsync(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				return BadRequest();

			if (!await _citiesRepository.DoesExistAsync(city))
				return NotFound();

			var date = _notificationsService.GetLastNotificationDate(city);

			return Ok(date);
		}

		private async Task<IActionResult> NotifyByOsAsync(NotificationRequestBody body)
		{
			if (!DeviceOsUtils.DoesNameExist(body.Os))
				return NotFound();

			var allDevices = await _devicesRepository.GetDevicesAsync();
			var devices = allDevices.Where(record => record.DeviceOs == body.Os).ToList();
			if (!devices.Any())
				return NotFound();

			var cityDevicesGroups = devices.Where(record => record.CityId.HasValue).GroupBy(device => device.CityId.Value);

			foreach (var cityDevicesGroup in cityDevicesGroups)
			{
				var city = await _citiesRepository.GetAsync(cityDevicesGroup.Key);

				if (city == null)
					continue;

				var notification = new Notification
				{
					Devices = cityDevicesGroup.Select(DeviceConverter.Convert),
					Message = body.Message,
					CityAlias = city.Alias
				};

				_notificationsService.Notify(notification);
			}

			return Ok();
		}

		private async Task<IActionResult> NotifyByDeviceAsync(NotificationRequestBody body)
		{
			var device = await _devicesRepository.GetDeviceAsync(body.DeviceToken);
			if (device == null)
				return NotFound();

			if (!device.CityId.HasValue)
				return NotFound();

			var city = await _citiesRepository.GetAsync(device.CityId.Value);

			if (city == null)
				return BadRequest();

			var notification = new Notification
			{
				Devices = new List<BaseDevice> { DeviceConverter.Convert(device) },
				Message = body.Message,
				CityAlias = city.Alias
			};

			_notificationsService.Notify(notification);

			return Ok();
		}
	}
}
