using System;
using System.Linq;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace SadWave.Events.Api.Common.Vk
{
	public static class PhotoExtensions
	{
		public static PhotoSize GetTheBiggest(this Photo photo)
		{
			return photo.Sizes.First(size => size.Square() >= photo.Sizes.Max(photoSize => photoSize.Square()));
		}

		private static ulong Square(this PhotoSize size)
		{
			return size.Width * size.Height;
		}
	}
}
