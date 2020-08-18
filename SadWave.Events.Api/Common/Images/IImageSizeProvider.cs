using System;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Common.Images
{
	public interface IImageSizeProvider
	{
		Task<ImageSize> GetSizeByUriAsync(Uri uri);
	}
}
