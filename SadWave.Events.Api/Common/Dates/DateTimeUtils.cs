using System;
using System.Globalization;

namespace SadWave.Events.Api.Common.Dates
{
	public static class DateTimeUtils
	{
		public static DateTime Parse(string stringDate)
		{
			if (string.IsNullOrWhiteSpace(stringDate))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(stringDate));

			var separatorIndex = stringDate.IndexOf(",", StringComparison.Ordinal);
			var clearStringDate = separatorIndex <= 0 ? stringDate : stringDate.Substring(0, separatorIndex);
			var formatProvider = CultureInfo.GetCultureInfo("ru-RU");
			if (DateTime.TryParseExact(clearStringDate, "dd MMMM", formatProvider, DateTimeStyles.None, out var date))
				return date;
			return DateTime.ParseExact(clearStringDate, "d MMMM", formatProvider);
		}

		public static bool TryParse(string stringDate, out DateTime date)
		{
			try
			{
				date = Parse(stringDate);
				return true;
			}
			catch (Exception)
			{
				date = DateTime.MinValue;
				return false;
			}
		}

		public static bool IsDate(string stringDate)
		{
			return TryParse(stringDate, out _);
		}
	}
}