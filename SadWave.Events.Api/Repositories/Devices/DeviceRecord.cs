namespace SadWave.Events.Api.Repositories.Devices
{
	public class DeviceRecord
	{
		public string DeviceOs { get; set; }

		public string DeviceToken { get; set; }

		public int? CityId { get; set; }

		public bool Sandbox { get; set; }
	}
}