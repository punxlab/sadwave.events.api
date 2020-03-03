using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Models.Devices;
using SadWave.Events.Api.Services.Devices;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/devices")]

	public class DevicesController : Controller
	{
		private readonly IDevicesService _devicesService;

		public DevicesController(IDevicesService devicesService)
		{
			_devicesService = devicesService ?? throw new ArgumentNullException(nameof(devicesService));
		}

		[HttpPost]
		public async Task<IActionResult> Register([FromBody]DeviceRequestBody device)
		{
			try
			{
				if (device == null || !ModelState.IsValid)
					return BadRequest();

				await _devicesService.AddAsync(device.DeviceToken, device.City, device.DeviceOs, device.ForSandbox);
				return Ok();
			}
			catch (UnknownOsException)
			{
				return BadRequest();
			}
			catch (CityNotFoundException)
			{
				return BadRequest();
			}
		}

		[HttpGet("{token}")]
		public async Task<IActionResult> GetDevice(string token)
		{
			if (string.IsNullOrEmpty(token))
				return BadRequest();

			var device = await _devicesService.GetDevice(token);

			if (device == null)
				return NotFound();

			return Ok(DeviceConverter.Convert(device));
		}
	}
}