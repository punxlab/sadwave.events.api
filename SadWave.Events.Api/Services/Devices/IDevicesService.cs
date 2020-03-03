using System.Threading.Tasks;

namespace SadWave.Events.Api.Services.Devices
{
	public interface IDevicesService
	{
		Task AddAsync(string deviceToken, string cityAlias, string deviceOs, bool sandbox);

		Task<Device> GetDevice(string token);

		Task UpdateDeviceAsync(string oldToken, string newToken);

		Task DeleteAsync(string token);
	}
}