using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace SadWave.Events.Api.Common.Html
{
	public class HtmlProvider
	{
		private readonly HtmlParser _parser;

		public HtmlProvider(HtmlParser parser)
		{
			_parser = parser ?? throw new ArgumentNullException(nameof(parser));
		}

		public async Task<IHtmlDocument> GetHtmlAsync(Uri address)
		{
			var request = (HttpWebRequest) WebRequest.Create(address);
			using (var response = (HttpWebResponse) await request.GetResponseAsync())
			{
				if (response.StatusCode != HttpStatusCode.OK)
					throw new InvalidOperationException(
						$"Cannot get HTML, because server returns response with code {response.StatusCode}");

				var receiveStream = response.GetResponseStream();

				if (receiveStream == null)
					return null;

				var encoding = response.CharacterSet == null ? Encoding.UTF8 : Encoding.GetEncoding(response.CharacterSet);
				using (var readStream = new StreamReader(receiveStream, encoding))
				{
					var html = (await readStream.ReadToEndAsync())
						.Replace("\r", string.Empty)
						.Replace("\n", string.Empty);

					return await _parser.ParseAsync(html);
				}
			}
		}
	}
}