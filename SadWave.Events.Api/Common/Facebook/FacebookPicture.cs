using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookPicture
	{
		[JsonProperty("data")]
		public FacebookPictureData Data { get; set; }
	}
}