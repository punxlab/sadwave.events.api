using NUnit.Framework;
using SadWave.Events.Api.Repositories.Events;
using System;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class EventsPhotoRepositoryTests : BaseDatabaseRepositoryTest
	{
		public EventsPhotoRepositoryTests() : base("events-photo-test.sqlite")
		{
		}

		[Test]
		public async Task SavePhotoForEvent()
		{
			var eventUri = new Uri("http://test.event.ru");
			var photoUri = new Uri("http://test.photo1.ru");
			var photoHeight = 10;
			var photoWidth = 20;

			var sut = CreateSut();
			await sut.SetPhotoAsync(new EventPhoto {
				EventUrl = eventUri,
				PhotoUrl = photoUri,
				PhotoHeight = photoHeight,
				PhotoWidth = photoWidth
			});

			var actialResult = await sut.GetEventPhotoAsync(eventUri);
			Assert.AreEqual(eventUri, actialResult.EventUrl);
			Assert.AreEqual(photoUri, actialResult.PhotoUrl);
			Assert.AreEqual(photoWidth, actialResult.PhotoWidth);
			Assert.AreEqual(photoHeight, actialResult.PhotoHeight);
		}

		[Test]
		public async Task ResetPhotoEvent()
		{
			var eventUri = new Uri("http://test.event.ru");
			var photoUri = new Uri("http://test.photo1.ru");
			var photoHeight = 10;
			var photoWidth = 20;

			var sut = CreateSut();
			await sut.SetPhotoAsync(new EventPhoto
			{
				EventUrl = eventUri,
				PhotoUrl = photoUri,
				PhotoHeight = photoHeight,
				PhotoWidth = photoWidth
			});

			await sut.SetPhotoAsync(new EventPhoto
			{
				EventUrl = eventUri,
				PhotoHeight = photoHeight,
				PhotoWidth = photoWidth
			});

			var actialResult = await sut.GetEventPhotoAsync(eventUri);
			Assert.AreEqual(eventUri, actialResult.EventUrl);
			Assert.IsNull(actialResult.PhotoUrl);
			Assert.AreEqual(photoWidth, actialResult.PhotoWidth);
			Assert.AreEqual(photoHeight, actialResult.PhotoHeight);
		}

		[Test]
		public async Task DeleteEventPhoto()
		{
			var eventUri = new Uri("http://test.event.ru");
			var photoUri = new Uri("http://test.photo1.ru");
			var photoHeight = 10;
			var photoWidth = 20;

			var sut = CreateSut();
			await sut.SetPhotoAsync(new EventPhoto
			{
				EventUrl = eventUri,
				PhotoUrl = photoUri,
				PhotoHeight = photoHeight,
				PhotoWidth = photoWidth
			});

			var actialResult1 = await sut.GetEventPhotoAsync(eventUri);
			Assert.IsNotNull(actialResult1);

			await sut.RemoveEventPhotoAsync(eventUri);

			var actialResult2 = await sut.GetEventPhotoAsync(eventUri);
			Assert.IsNull(actialResult2);
		}

		private EventsPhotoRepository CreateSut()
		{
			return new EventsPhotoRepository(ConnectionFactory);
		}
	}
}
