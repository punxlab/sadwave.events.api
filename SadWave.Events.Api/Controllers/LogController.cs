using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Models.Log;
using SadWave.Events.Api.Common.Logger;
using System.Text;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/log")]
	public class LogController : Controller
	{
		private readonly ILogger _logger;

		public LogController(ILogger logger)
		{
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		[HttpPost("error")]
		public IActionResult LogError([FromBody]ErrorLogMessage logMessage)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			_logger.Error(Convert(logMessage));
			return Ok();
		}

		private string Convert(ErrorLogMessage message)
		{
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendLine($"Message: {message.Message}");

			if(!string.IsNullOrWhiteSpace(message.Exception))
			{
				messageBuilder.AppendLine($"Exception: {message.Exception}");
			}

			if (!string.IsNullOrWhiteSpace(message.Tag))
			{
				messageBuilder.AppendLine($"Tag: {message.Tag}");
			}

			if (message.DeviceInformation != null)
			{
				messageBuilder.AppendLine("Device Information");

				if (!string.IsNullOrWhiteSpace(message.DeviceInformation.Manufacturer))
				{
					messageBuilder.AppendLine($"Manufacturer: {message.DeviceInformation.Manufacturer}");
				}

				if (!string.IsNullOrWhiteSpace(message.DeviceInformation.Model))
				{
					messageBuilder.AppendLine($"Model: {message.DeviceInformation.Model}");
				}

				if (!string.IsNullOrWhiteSpace(message.DeviceInformation.Platform))
				{
					messageBuilder.AppendLine($"Platform: {message.DeviceInformation.Platform}");
				}

				if (!string.IsNullOrWhiteSpace(message.DeviceInformation.Version))
				{
					messageBuilder.AppendLine($"Version: {message.DeviceInformation.Version}");
				}
			}

			return messageBuilder.ToString();
		}
	}
}
