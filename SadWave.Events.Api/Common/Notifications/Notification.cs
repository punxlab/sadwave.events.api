using System.Collections.Generic;

namespace SadWave.Events.Api.Common.Notifications
{
	public class Notification
	{
		public IEnumerable<BaseDevice> Devices { get; set; }

		public string Message { get; set; }

		public string CityAlias { get; set; }
	}
}
