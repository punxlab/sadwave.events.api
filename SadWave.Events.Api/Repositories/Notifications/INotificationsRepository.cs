using System;

namespace SadWave.Events.Api.Repositories.Notifications
{
	public interface INotificationsRepository
	{
		void SetDate(string cityAlias, DateTime date);

		DateTime GetDate(string cityAlias);

		void DeleteDate(string cityAlias);

	}
}