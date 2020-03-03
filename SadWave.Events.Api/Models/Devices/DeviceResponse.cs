using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Devices
{
	public class DeviceResponse
	{
		[JsonProperty("device")]
		public string DeviceToken { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }
	}
}