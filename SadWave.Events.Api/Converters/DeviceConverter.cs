using System;
using SadWave.Events.Api.Common.Notifications;
using SadWave.Events.Api.Models.Devices;
using SadWave.Events.Api.Repositories.Devices;
using SadWave.Events.Api.Services.Devices;

namespace SadWave.Events.Api.Converters
{
	public static class DeviceConverter
	{
		public static BaseDevice Convert(DeviceRecord record)
		{
			if (record == null)
				throw new ArgumentNullException(nameof(record));

			switch (record.DeviceOs)
			{
				case DeviceOsName.Android:
					return new AndroidDevice { Token = record.DeviceToken };
				case DeviceOsName.Ios:
					return new IosDevice { Token = record.DeviceToken, Mode = record.Sandbox ? Mode.Sandbox : Mode.Production };
				default:
					throw new ArgumentOutOfRangeException(nameof(record.DeviceOs));
			}
		}

		public static DeviceResponse Convert(Device device)
		{
			if (device == null) throw new ArgumentNullException(nameof(device));

			return new DeviceResponse
			{
				City = device.CityAlias,
				DeviceToken = device.DeviceOs
			};
		}
	}
}
