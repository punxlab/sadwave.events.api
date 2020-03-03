using System;

namespace SadWave.Events.Api.Common.Notifications
{
	public class NotificationSuccessEventArgs : EventArgs
	{
		public NotificationSuccessEventArgs(Notification notification)
		{
			Notification = notification ?? throw new ArgumentNullException(nameof(notification));
		}

		public Notification Notification { get; }
	}
}