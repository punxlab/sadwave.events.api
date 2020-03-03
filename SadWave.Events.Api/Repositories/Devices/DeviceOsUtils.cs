using System;

namespace SadWave.Events.Api.Repositories.Devices
{
	public static class DeviceOsUtils
	{
		public static string GetName(DeviceOs deviceOs)
		{
			switch (deviceOs)
			{
				case DeviceOs.Ios:
					return DeviceOsName.Ios;
				case DeviceOs.Android:
					return DeviceOsName.Android;
				default:
					return null;
			}
		}

		public static DeviceOs? GetOs(string deviceOs)
		{
			if (string.IsNullOrWhiteSpace(deviceOs))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceOs));

			var os = deviceOs.ToLower().Trim();
			if (os == DeviceOsName.Android)
				return DeviceOs.Android;

			if (os == DeviceOsName.Ios)
				return DeviceOs.Ios;

			return null;
		}

		public static bool DoesNameExist(string name)
		{
			return DeviceOsName.Android.Equals(name, StringComparison.OrdinalIgnoreCase)
				|| DeviceOsName.Ios.Equals(name, StringComparison.OrdinalIgnoreCase);
		}
	}
}