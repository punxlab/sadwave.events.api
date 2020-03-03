using System;

namespace SadWave.Events.Api.Common.Notifications
{
	public interface IPushNotifier
	{
		void Notify(Notification notification);

		event EventHandler<NotificationSuccessEventArgs> NotificationSuccess;

		event EventHandler<NotificationFailedEventArgs> NotificationFailed;
	}
}