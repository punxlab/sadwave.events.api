using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;

namespace SadWave.Events.Api.Common.Images
{
	public class ImageSizeProvider : IImageSizeProvider
	{
		public async Task<ImageSize> GetSizeByUriAsync(Uri uri)
		{
			if (uri is null)
				throw new ArgumentNullException(nameof(uri));

			using (var client = new WebClient())
			{
				var data = await client.DownloadDataTaskAsync(uri);
				using (var stream = new MemoryStream(data))
				{
					var img = Image.FromStream(stream);
					return new ImageSize
					{
						Height = img.Height,
						Width = img.Width,
					};
				}
			}
		}
	}
}
