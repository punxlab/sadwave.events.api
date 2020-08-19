using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SadWave.Events.Api.Repositories.Cities;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class CityRepositoryTests : BaseDatabaseRepositoryTest
	{
		public CityRepositoryTests() : base("city-test.sqlite")
		{
		}

		[Test]
		public async Task GetCityByAliasTest()
		{
			const string expectedAlias = "msk";
			const string expectedName = "Moscow";
			const string expectedUri = "http://test.moscow.com";

			var sut = CreateSut();

			await sut.AddAsync(expectedAlias, expectedName, expectedUri);

			var actual = await sut.GetAsync(expectedAlias);

			actual.Should().NotBeNull();
			actual.Alias.Should().NotBeNull();
			actual.Alias.Should().Be(expectedAlias);
			actual.Name.Should().NotBeNull();
			actual.Name.Should().Be(expectedName);
			actual.PageUrl.Should().Be(expectedUri);
		}

		[Test]
		public async Task AddCanUpdateCityByAliasTest()
		{
			const string expectedAlias = "msk";
			const string insertedName = "Moscow";
			const string insertedUri = "http://test.moscow.com";

			const string updatedName = "Москва";
			const string updatedUri = "http://new.test.moscow.com";

			var sut = CreateSut();

			await sut.AddAsync(expectedAlias, insertedName, insertedUri);

			var inserted = await sut.GetAsync(expectedAlias);

			inserted.Should().NotBeNull();
			inserted.Alias.Should().NotBeNull();
			inserted.Alias.Should().Be(expectedAlias);
			inserted.Name.Should().NotBeNull();
			inserted.Name.Should().Be(insertedName);
			inserted.PageUrl.Should().Be(insertedUri);

			await sut.AddAsync(expectedAlias, updatedName, updatedUri);

			var updated = await sut.GetAsync(expectedAlias);
			updated.Should().NotBeNull();
			updated.Alias.Should().NotBeNull();
			updated.Alias.Should().Be(expectedAlias);
			updated.Name.Should().NotBeNull();
			updated.Name.Should().Be(updatedName);
			updated.PageUrl.Should().Be(updatedUri);
		}

		[Test]
		public async Task GetCityByIdTest()
		{
			const string expectedAlias = "msk";
			const string expectedName = "Moscow";
			const string expectedUri = "http://test.moscow.com";

			var sut = CreateSut();

			await sut.AddAsync(expectedAlias, expectedName, expectedUri);

			var resultByAlias = await sut.GetAsync(expectedAlias);

			var actual = await sut.GetAsync(resultByAlias.Id);

			actual.Should().NotBeNull();
			actual.Alias.Should().NotBeNull();
			actual.Alias.Should().Be(expectedAlias);
			actual.Name.Should().NotBeNull();
			actual.Name.Should().Be(expectedName);
			actual.PageUrl.Should().Be(expectedUri);
		}

		[Test]
		public async Task GetCitiesTest()
		{
			const string firstExpectedAlias = "msk";
			const string firstExpectedName = "Moscow";
			const string firstExpectedUri = "http://msk-test.moscow.com";

			const string secondExpectedAlias = "spb";
			const string secondExpectedName = "Spb";
			const string secondExpectedUri = "http://spb-test.moscow.com";

			var expected = new List<CityRecord>
			{
				new CityRecord
				{
					Id = 1,
					Alias = firstExpectedAlias,
					Name = firstExpectedName,
					PageUrl = firstExpectedUri
				},
				new CityRecord
				{
					Id = 2,
					Alias = secondExpectedAlias,
					Name = secondExpectedName,
					PageUrl = secondExpectedUri
				}
			};

			var sut = CreateSut();

			await sut.AddAsync(firstExpectedAlias, firstExpectedName, firstExpectedUri);
			await sut.AddAsync(secondExpectedAlias, secondExpectedName, secondExpectedUri);

			var actual = await sut.GetAsync();

			expected.Should().BeEquivalentTo(actual);
		}

		[Test]
		public async Task DoesCityExistReturnsTrueIfCityExistsTest()
		{
			const string expectedAlias = "test";
			const string expectedName = "Test";
			const string expectedUri = "http://test.moscow.com";

			var sut = CreateSut();

			await sut.AddAsync(expectedAlias, expectedName, expectedUri);

			var actual = await sut.DoesExistAsync(expectedAlias);

			actual.Should().BeTrue();
		}

		[Test]
		public async Task DoesCityExistReturnsFalseIfCityDoesntExistTest()
		{
			var sut = CreateSut();

			var actual = await sut.DoesExistAsync("unknown-city");

			actual.Should().BeFalse();
		}

		private CitiesRepository CreateSut()
		{
			return new CitiesRepository(ConnectionFactory);
		}
	}
}