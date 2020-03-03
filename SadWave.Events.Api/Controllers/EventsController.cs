using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Events;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/events")]
	public class EventsController : Controller
	{
		private readonly IEventsService _eventsService;

		public EventsController(IEventsService eventsService)
		{
			_eventsService = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
		}

		[HttpGet("{city}")]
		public async Task<IActionResult> GetEvents(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				return BadRequest();

			try
			{
				var events = await _eventsService.GetCityEventsAsync(city);
				return Ok(events.Select(EventConverter.Convert));
			}
			catch (CityNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> SaveEvents()
		{
			await _eventsService.SaveEventsAsync();
			return Ok();
		}

		[HttpDelete]
		[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> DeleteEvents()
		{
			await _eventsService.DeleteEventsAsync();
			return Ok();
		}
	}
}