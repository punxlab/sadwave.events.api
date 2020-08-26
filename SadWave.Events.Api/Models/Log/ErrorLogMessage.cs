using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SadWave.Events.Api.Models.Log
{
	public class ErrorLogMessage
	{
		[Required]
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("exception")]
		public string Exception { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }

		[JsonProperty("device")]
		public DeviceInformation DeviceInformation { get; set; }
	}
}