using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookPlace
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("location")]
		public FacebookLocation Location { get; set; }
	}
}