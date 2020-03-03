using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookPictureData
	{
		[JsonProperty("height")]
		public int Height { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}