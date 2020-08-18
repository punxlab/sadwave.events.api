using NUnit.Framework;
using SadWave.Events.Api.Common.Images;
using System;

namespace SadWave.Events.Api.Tests.Common.Images
{
	[TestFixture]
	public class ImageSizeProviderTests
	{
		[Test]
		public void ProvideByUriReturnsCorrectSize()
		{
			var expectedWidth = 600;
			var expectedHeight = 400;

			var sut = new ImageSizeProvider();
			var actualResult = sut.GetSizeByUriAsync(new Uri("https://dummyimage.com/600x400")).GetAwaiter().GetResult();

			Assert.AreEqual(expectedWidth, actualResult.Width);
			Assert.AreEqual(expectedHeight, actualResult.Height);
		}
	}
}