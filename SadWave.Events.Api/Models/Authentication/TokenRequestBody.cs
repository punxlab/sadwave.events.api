using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Authentication
{
	public class TokenRequestBody
	{
		[JsonProperty("login")]
		[Required]
		public string Login { get; set; }

		[JsonProperty("password")]
		[Required]
		public string Password { get; set; }
	}
}
