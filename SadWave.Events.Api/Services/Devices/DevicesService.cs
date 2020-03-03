using System;
using System.Threading.Tasks;
using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Repositories.Devices;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Services.Devices
{
	public class DevicesService : IDevicesService
	{
		private readonly IDevicesRepository _devicesRepository;
		private readonly ICitiesRepository _citiesRepository;

		public DevicesService(IDevicesRepository devicesRepository, ICitiesRepository citiesRepository)
		{
			_devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
			_citiesRepository = citiesRepository ?? throw new ArgumentNullException(nameof(citiesRepository));
		}

		public Task AddAsync(string deviceToken, string cityAlias, string deviceOs, bool sandbox)
		{
			if (string.IsNullOrWhiteSpace(deviceToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceToken));
			if (string.IsNullOrWhiteSpace(deviceOs))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceOs));
			if (!DeviceOsUtils.DoesNameExist(deviceOs))
				throw new UnknownOsException();

			return AddDeviceAsync(deviceToken, cityAlias, deviceOs, sandbox);
		}

		public Task<Device> GetDevice(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

			return GetDeviceAsync(token);
		}

		public Task UpdateDeviceAsync(string oldToken, string newToken)
		{
			if (string.IsNullOrWhiteSpace(oldToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(oldToken));
			if (string.IsNullOrWhiteSpace(newToken))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(newToken));

			return _devicesRepository.UpdateDeviceAsync(oldToken, newToken);
		}

		public Task DeleteAsync(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

			return _devicesRepository.DeleteAsync(token);
		}

		private async Task<Device> GetDeviceAsync(string token)
		{
			var deviceRecord = await _devicesRepository.GetDeviceAsync(token);
			if (deviceRecord == null)
				return null;

			CityRecord cityRecord = null;
			if (deviceRecord.CityId.HasValue)
			{
				cityRecord = await _citiesRepository.GetAsync(deviceRecord.CityId.Value);
			}

			return new Device
			{
				DeviceToken = deviceRecord.DeviceToken,
				CityAlias = cityRecord?.Alias,
				DeviceOs = deviceRecord.DeviceOs
			};
		}

		private async Task AddDeviceAsync(string deviceToken, string cityAlias, string deviceOs, bool sandbox)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
			{
				await _devicesRepository.AddAsync(deviceToken, deviceOs, sandbox);
			}
			else
			{
				var city = await _citiesRepository.GetAsync(cityAlias);
				if (city == null)
					throw new CityNotFoundException();

				await _devicesRepository.AddAsync(deviceToken, city.Id, deviceOs, sandbox);
			}
		}
	}
}
