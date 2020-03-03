using System;

namespace SadWave.Events.Api.Common.Notifications
{
	public class NotificationFailedEventArgs : EventArgs
	{
		public NotificationFailedEventArgs(Exception exception)
		{
			Exception = exception ?? throw new ArgumentNullException(nameof(exception));
		}

		public Exception Exception { get; }
	}
}