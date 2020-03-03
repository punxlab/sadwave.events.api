using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Devices
{
	public class DeviceRequestBody
	{
		[Required]
		[JsonProperty("device")]
		public string DeviceToken { get; set; }

		[Required]
		[JsonProperty("os")]
		public string DeviceOs { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("sandbox")]
		public bool ForSandbox { get; set; }
	}
}
