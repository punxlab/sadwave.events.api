using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Log
{
	public class DeviceInformation
	{
		[JsonProperty("model")]
		public string Model { get; set; }

		[JsonProperty("manufacturer")]
		public string Manufacturer { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("platform")]
		public string Platform { get; set; }
	}
}