using Newtonsoft.Json;

namespace SadWave.Events.Api.Common.Facebook
{
	public class FacebookErrorResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("error_subcode")]
		public string SubCode { get; set; }

		[JsonProperty("fbtrace_id")]
		public string TraceId { get; set; }
	}
}