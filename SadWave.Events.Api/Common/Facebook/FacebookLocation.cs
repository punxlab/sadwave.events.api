using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookLocation
	{
		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("street")]
		public string Street { get; set; }
	}
}