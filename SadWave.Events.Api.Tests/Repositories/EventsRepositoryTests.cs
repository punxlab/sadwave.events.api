using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SadWave.Events.Api.Common.Storages;
using SadWave.Events.Api.Repositories.Events;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class EventsRepositoryTests : BaseCacheTest
	{
		[Test]
		public void GetCityEventsTest()
		{
			var expectedCity = "test-city";
			var expected = new List<EventRecord>
			{
				new EventRecord
				{
					Address = "Test address 1",
					Date = new EventDateRecord
					{
						Date = DateTime.Today,
						Time = new TimeSpan(10, 10, 10)
					},
					Name = "Name 1",
					Description = "Description 1",
					Overview = "Overview 1",
					Photo = new Uri("http://test.photo1.ru"),
					Url = new Uri("http://test1.ru")
				},
				new EventRecord
				{
					Address = "Test address 2",
					Date = new EventDateRecord
					{
						Date = DateTime.Today,
						Time = new TimeSpan(12, 12, 12)
					},
					Name = "Name 2",
					Description = "Description 2",
					Overview = "Overview 1",
					Photo = new Uri("http://test.photo2.ru"),
					Url = new Uri("http://test2.ru")
				}
			};
			var sut = CreateSut();
			sut.SetEvents(expected, expectedCity);
			var actual = sut.GetCityEvents(expectedCity);
			actual.Should().BeEquivalentTo(expected);
		}

		[Test]
		public void RemoveEventsTest()
		{
			var expectedCity = "test-city";
			var expected = new List<EventRecord>
			{
				new EventRecord
				{
					Address = "Test address 1",
					Date = new EventDateRecord
					{
						Date = DateTime.Today,
						Time = new TimeSpan(10, 10, 10)
					},
					Name = "Name 1",
					Description = "Description 1",
					Overview = "Overview 1",
					Photo = new Uri("http://test.photo1.ru"),
					Url = new Uri("http://test1.ru")
				},
				new EventRecord
				{
					Address = "Test address 2",
					Date = new EventDateRecord
					{
						Date = DateTime.Today,
						Time = new TimeSpan(12, 12, 12)
					},
					Name = "Name 2",
					Description = "Description 2",
					Overview = "Overview 1",
					Photo = new Uri("http://test.photo2.ru"),
					Url = new Uri("http://test2.ru")
				}
			};
			var sut = CreateSut();
			sut.SetEvents(expected, expectedCity);

			var actual = sut.GetCityEvents(expectedCity);
			actual.Count().Should().Be(2);

			sut.RemoveEvents(expectedCity);

			actual = sut.GetCityEvents(expectedCity);
			actual.Count().Should().Be(0);
		}

		private EventsRepository CreateSut()
		{
			return new EventsRepository(FileCacheStorage);
		}
	}
}
