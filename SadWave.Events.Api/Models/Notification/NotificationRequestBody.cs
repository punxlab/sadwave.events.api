using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Notification
{
	public class NotificationRequestBody
	{
		[JsonProperty("os")]
		public string Os { get; set; }

		[JsonProperty("device")]
		public string DeviceToken { get; set; }

		[JsonProperty("message")]
		[Required]
		public string Message { get; set; }
	}
}
