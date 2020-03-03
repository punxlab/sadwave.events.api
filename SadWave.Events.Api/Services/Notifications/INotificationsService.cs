using System;
using SadWave.Events.Api.Common.Notifications;

namespace SadWave.Events.Api.Services.Notifications
{
	public interface INotificationsService
	{
		void Notify(Notification notification);

		void DeleteLastNotificationDate(string city);

		DateTime GetLastNotificationDate(string city);

		bool WasCityNotifiedToday(string cityAlias);

		void MarkCityAsNotified(string cityAlias);
	}
}