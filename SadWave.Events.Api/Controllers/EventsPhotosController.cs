using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Models.Events;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Events;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/photos")]
	public class EventsPhotosController : Controller
	{
		private readonly IEventsService _eventsService;

		public EventsPhotosController(IEventsService eventsService)
		{
			_eventsService = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
		}

		[HttpPost]
		//[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> SetEventPhoto([FromBody] SetEventPhotoRequestBody body)
		{
			if (body == null)
				return BadRequest("Body is empty.");

			if (body.EventUrl == null || string.IsNullOrWhiteSpace(body.EventUrl.ToString()))
				return BadRequest("Event URL is null or empty.");

			await _eventsService.SetCustomEventPhotoAsync(body.EventUrl, body.PhotoUri);
			return Ok();
		}
	}
}