using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Services.Cities;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/cities")]
	public class CitiesController : Controller
	{
		private readonly ICitiesService _citiesService;

		public CitiesController(ICitiesService citiesService)
		{
			_citiesService = citiesService ?? throw new ArgumentNullException(nameof(citiesService));
		}

		[HttpGet]
		public async Task<IActionResult> GetCities()
		{
			var cities = await _citiesService.GetAsync();
			return Ok(cities);
		}
	}
}