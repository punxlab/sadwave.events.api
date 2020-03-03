using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Repositories.Devices;

namespace SadWave.Events.Api.Tests.Repositories
{
	[TestFixture]
	public class DevicesRepositoryTests : BaseDatabaseRepositoryTest
	{
		public DevicesRepositoryTests() : base("devices-test.sqlite")
		{
		}

		[Test]
		public async Task GetDeviceByTokenTest()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedDeviceToken = "test-device-token";
			const bool expectedSandboxValue = false;
			const string expectedDeviceOs = DeviceOsName.Android;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var sut = CreateSut();
			await sut.AddAsync(expectedDeviceToken, expectedCity.Id, expectedDeviceOs, expectedSandboxValue);
			var actual = await sut.GetDeviceAsync(expectedDeviceToken);

			actual.Should().NotBeNull();
			actual.DeviceOs.Should().Be(expectedDeviceOs);
			actual.CityId.Should().Be(expectedCity.Id);
			actual.DeviceToken.Should().Be(expectedDeviceToken);
			actual.Sandbox.Should().Be(expectedSandboxValue);
		}

		[Test]
		public async Task AddDeviceDoesntAddIfOsIsIncorrectTest()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedDeviceToken = "test-device-token";
			const bool expectedSandboxValue = false;
			const string incorrectOs = "incorrect-os";

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var sut = CreateSut();
			await sut.AddAsync(expectedDeviceToken, expectedCity.Id, incorrectOs, expectedSandboxValue);
			var actual = await sut.GetDeviceAsync(expectedDeviceToken);

			actual.Should().BeNull();
		}

		[Test]
		public async Task GetDevicesByCityAliasTest()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedFirstDeviceToken = "test-device-token-1";
			const string expectedSecondDeviceToken = "test-device-token-2";
			const bool expectedFirstDeviceSandboxValue = false;
			const bool expectedSecondDeviceSandboxValue = true;

			const string expectedFirstDeviceOs = DeviceOsName.Android;
			const string expectedSecondDeviceOs = DeviceOsName.Ios;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var expected = new List<DeviceRecord>
			{
				new DeviceRecord
				{
					DeviceToken = expectedFirstDeviceToken,
					DeviceOs = expectedFirstDeviceOs,
					CityId = expectedCity.Id,
					Sandbox = expectedFirstDeviceSandboxValue
				},
				new DeviceRecord
				{
					DeviceToken = expectedSecondDeviceToken,
					DeviceOs = expectedSecondDeviceOs,
					CityId = expectedCity.Id,
					Sandbox = expectedSecondDeviceSandboxValue
				}
			};

			var sut = CreateSut();
			await sut.AddAsync(expectedFirstDeviceToken, expectedCity.Id, expectedFirstDeviceOs, expectedFirstDeviceSandboxValue);
			await sut.AddAsync(expectedSecondDeviceToken, expectedCity.Id, expectedSecondDeviceOs, expectedSecondDeviceSandboxValue);

			var actual = await sut.GetDevicesAsync(expectedCityAlias);
			actual.Should().BeEquivalentTo(expected);
		}

		[Test]
		public async Task GetAllDevicesTest()
		{
			const string expectedFirstCityAlias = "test-city-1";
			const string expectedSecondCityAlias = "test-city-2";

			const string expectedFirstCityName = "Test-1";
			const string expectedSecondCityName = "Test-2";

			const string expectedFirstCityPageUrl = "http://test-1.url.ru";
			const string expectedSecondCityPageUrl = "http://test-2.url.ru";

			const string expectedFirstDeviceToken = "test-device-token-1";
			const string expectedSecondDeviceToken = "test-device-token-2";

			const bool expectedFirstDeviceSandboxValue = false;
			const bool expectedSecondDeviceSandboxValue = true;

			const string expectedFirstDeviceOs = DeviceOsName.Android;
			const string expectedSecondDeviceOs = DeviceOsName.Ios;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedFirstCityAlias, expectedFirstCityName, expectedFirstCityPageUrl);
			await cityRepository.AddAsync(expectedSecondCityAlias, expectedSecondCityName, expectedSecondCityPageUrl);

			var expectedFirstCity = await cityRepository.GetAsync(expectedFirstCityAlias);
			var expectedSecondCity = await cityRepository.GetAsync(expectedSecondCityAlias);

			var expected = new List<DeviceRecord>
			{
				new DeviceRecord
				{
					DeviceToken = expectedFirstDeviceToken,
					DeviceOs = expectedFirstDeviceOs,
					CityId = expectedFirstCity.Id,
					Sandbox = expectedFirstDeviceSandboxValue
				},
				new DeviceRecord
				{
					DeviceToken = expectedSecondDeviceToken,
					DeviceOs = expectedSecondDeviceOs,
					CityId = expectedSecondCity.Id,
					Sandbox = expectedSecondDeviceSandboxValue
				}
			};

			var sut = CreateSut();
			await sut.AddAsync(expectedFirstDeviceToken, expectedFirstCity.Id, expectedFirstDeviceOs, expectedFirstDeviceSandboxValue);
			await sut.AddAsync(expectedSecondDeviceToken, expectedSecondCity.Id, expectedSecondDeviceOs, expectedSecondDeviceSandboxValue);

			var actual = await sut.GetDevicesAsync();
			actual.Should().BeEquivalentTo(expected);
		}

		[Test]
		public async Task UpdateDeviceTokenTest()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedFirstDeviceToken = "test-device-token";
			const string expectedSecondDeviceToken = "test-device-token";
			const bool expectedSandboxValue = false;
			const string expectedDeviceOs = DeviceOsName.Android;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var sut = CreateSut();
			await sut.AddAsync(expectedFirstDeviceToken, expectedCity.Id, expectedDeviceOs, expectedSandboxValue);
			await sut.UpdateDeviceAsync(expectedFirstDeviceToken, expectedSecondDeviceToken);
			var actual = await sut.GetDeviceAsync(expectedFirstDeviceToken);

			actual.Should().NotBeNull();
			actual.DeviceOs.Should().Be(expectedDeviceOs);
			actual.CityId.Should().Be(expectedCity.Id);
			actual.DeviceToken.Should().Be(expectedSecondDeviceToken);
			actual.Sandbox.Should().Be(expectedSandboxValue);
		}

		[Test]
		public async Task DeleteDeviceTest()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedDeviceToken = "test-device-token";
			const bool expectedSandboxValue = false;
			const string expectedDeviceOs = DeviceOsName.Android;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var sut = CreateSut();
			await sut.AddAsync(expectedDeviceToken, expectedCity.Id, expectedDeviceOs, expectedSandboxValue);

			var firstActual = await sut.GetDeviceAsync(expectedDeviceToken);
			firstActual.Should().NotBeNull();

			await sut.DeleteAsync(expectedDeviceToken);

			var secondActual = await sut.GetDeviceAsync(expectedDeviceToken);
			secondActual.Should().BeNull();
		}

		[Test]
		public async Task DoesDeviceExistReturnsTrue()
		{
			const string expectedCityAlias = "test-city";
			const string expectedCityName = "Test";
			const string expectedCityPageUrl = "http://test.url.ru";
			const string expectedDeviceToken = "test-device-token";
			const bool expectedSandboxValue = false;
			const string expectedDeviceOs = DeviceOsName.Android;

			var cityRepository = CreateCitiesRepository();
			await cityRepository.AddAsync(expectedCityAlias, expectedCityName, expectedCityPageUrl);
			var expectedCity = await cityRepository.GetAsync(expectedCityAlias);

			var sut = CreateSut();
			await sut.AddAsync(expectedDeviceToken, expectedCity.Id, expectedDeviceOs, expectedSandboxValue);

			var actualResult = await sut.DoesExistAsync(expectedDeviceToken);
			actualResult.Should().BeTrue();
		}

		[Test]
		public async Task DoesDeviceExistReturnsFalse()
		{
			const string unknownDeviceToken = "unknown-test-device-token";

			var sut = CreateSut();

			var actualResult = await sut.DoesExistAsync(unknownDeviceToken);
			actualResult.Should().BeFalse();
		}

		private CitiesRepository CreateCitiesRepository()
		{
			return new CitiesRepository(ConnectionTestory);
		}

		private DevicesRepository CreateSut()
		{
			return  new DevicesRepository(ConnectionTestory);
		}
	}
}
