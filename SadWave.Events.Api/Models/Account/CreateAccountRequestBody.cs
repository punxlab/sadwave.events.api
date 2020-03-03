using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SadWave.Events.Api.Models.Account
{
	public class CreateAccountRequestBody
	{
		[JsonProperty("login")]
		[Required]
		public string Login { get; set; }

		[JsonProperty("password")]
		[Required]
		public string Password { get; set; }
	}
}
