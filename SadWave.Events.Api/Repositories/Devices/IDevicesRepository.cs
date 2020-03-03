using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadWave.Events.Api.Repositories.Devices
{
	public interface IDevicesRepository
	{
		Task AddAsync(string deviceToken, int cityId, string deviceOs, bool sandbox);

		Task AddAsync(string deviceToken, string deviceOs, bool sandbox);

		Task<IEnumerable<DeviceRecord>> GetDevicesAsync(string cityAlias);

		Task<DeviceRecord> GetDeviceAsync(string token);

		Task<bool> DoesExistAsync(string token);

		Task<IEnumerable<DeviceRecord>> GetDevicesAsync();

		Task UpdateDeviceAsync(string oldToken, string newToken);

		Task DeleteAsync(string token);
	}
}