using System;
using FluentAssertions;
using NUnit.Framework;
using SadWave.Events.Api.Repositories.Notifications;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class NotificationsRepositoryTests : BaseCacheTest
	{
		[Test]
		public void GetNotificationDateTest()
		{
			const string firstCityAlias = "someCity1";
			const string secondCityAlias = "someCity2";

			var firstExpectedDate = new DateTime(2018, 01, 01);
			var secondExpectedDate = new DateTime(2018, 02, 02);

			var sut = CreateSut();

			sut.SetDate(firstCityAlias, firstExpectedDate);
			var firstActualResult = sut.GetDate(firstCityAlias);

			sut.SetDate(secondCityAlias, secondExpectedDate);
			var secondActualResult = sut.GetDate(secondCityAlias);

			firstActualResult.Should().Be(firstActualResult);
			secondActualResult.Should().Be(secondExpectedDate);
		}

		[Test]
		public void SetNotificationDateIsAbleToUpdateTest()
		{
			const string expectedCityAlias = "someCity";

			var expectedDate1 = new DateTime(2018, 01, 01);
			var expectedDate2 = new DateTime(2018, 02, 02);

			var sut = CreateSut();

			sut.SetDate(expectedCityAlias, expectedDate1);
			sut.SetDate(expectedCityAlias, expectedDate2);

			var actual = sut.GetDate(expectedCityAlias);

			actual.Should().Be(expectedDate2);
		}

		[Test]
		public void SGetNotificationDateReturnsDefaultDateIfItIsFirstRecordTest()
		{
			var sut = CreateSut();
			var actual = sut.GetDate("someCoty");
			actual.Should().Be(default(DateTime));
		}

		private NotificationsRepository CreateSut()
		{
			return new NotificationsRepository(FileCacheStorage);
		}
	}
}
