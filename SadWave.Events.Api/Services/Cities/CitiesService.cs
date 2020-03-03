using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SadWave.Events.Api.Converters;
using SadWave.Events.Api.Repositories.Cities;

namespace SadWave.Events.Api.Services.Cities
{
	public class CitiesService : ICitiesService
	{
		private readonly ICitiesRepository _repository;

		public CitiesService(ICitiesRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public Task<bool> DoesExistAsync(string alias)
		{
			if (string.IsNullOrWhiteSpace(alias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));

			return _repository.DoesExistAsync(alias);
		}

		public Task<City> GetAsync(string alias)
		{
			if (string.IsNullOrWhiteSpace(alias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));

			return GetCityAsync(alias);
		}

		public async Task<City> GetAsync(int id)
		{
			var result = await _repository.GetAsync(id);
			return CityConverter.Convert(result);
		}

		public async Task<IEnumerable<City>> GetAsync()
		{
			var result = await _repository.GetAsync();
			return result.Select(CityConverter.Convert);
		}

		public Task AddAsync(string alias, string name, string uri)
		{
			if (string.IsNullOrWhiteSpace(alias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
			if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(uri));
			if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				throw new ArgumentException("Value should be an absolute correct format uri.", nameof(uri));

			return AddCityAsync(alias, name, uri);
		}

		private async Task AddCityAsync(string alias, string name, string uri)
		{
			await _repository.AddAsync(alias, name, uri);
		}

		private async Task<City> GetCityAsync(string alias)
		{
			var result = await _repository.GetAsync(alias);
			return CityConverter.Convert(result);
		}
	}
}
