using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Services.Cities;

namespace SadWave.Events.Api.Converters
{
	public static class CityConverter
	{
		public static City Convert(CityRecord record)
		{
			if (record == null)
				return null;

			return new City
			{
				Id = record.Id,
				Alias = record.Alias,
				Name = record.Name,
				PageUrl = record.PageUrl
			};
		}
	}
}