using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Common;
using PushSharp.Google;

namespace SadWave.Events.Api.Common.Notifications
{
	public class PushNotifier : IPushNotifier
	{
		private readonly GcmServiceBrokerFactory _gcmServiceBrokerFactory;
		private readonly ApnsServiceBrokerFactory _apnsServiceBrokerFactory;

		private const string EventType = "NewEvents";

		public event EventHandler<NotificationFailedEventArgs> NotificationFailed;
		public event EventHandler<NotificationSuccessEventArgs> NotificationSuccess;

		public PushNotifier(ApnsServiceBrokerFactory apnsBrokerFactory, GcmServiceBrokerFactory gcmServiceBrokerFactory)
		{
			_gcmServiceBrokerFactory = gcmServiceBrokerFactory ?? throw new ArgumentNullException(nameof(gcmServiceBrokerFactory));
			_apnsServiceBrokerFactory = apnsBrokerFactory ?? throw new ArgumentNullException(nameof(apnsBrokerFactory));
		}

		public void Notify(Notification notification)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			var androidDevices = notification.Devices.Where(device => device is AndroidDevice).Cast<AndroidDevice>();
			var iosDevices = notification.Devices.Where(device => device is IosDevice).Cast<IosDevice>();

			Notify(androidDevices.ToList(), notification);
			Notify(iosDevices.ToList(), notification);
		}

		private void Notify(IReadOnlyCollection<AndroidDevice> androidDevices, Notification notification)
		{
			if (androidDevices.Count == 0)
				return;

			var broker = _gcmServiceBrokerFactory.Create();
			DoWithBroker(broker, notification, () =>
			{
				broker.QueueNotification(new GcmNotification
				{
					RegistrationIds = androidDevices.Select(device => device.Token).ToList(),
					Data = CreateAndroidMessage(notification.Message)
				});
			});
		}

		private void Notify(IReadOnlyCollection<IosDevice> iosDevices, Notification notification)
		{
			if (iosDevices.Count == 0)
				return;

			foreach (var deviceGroup in iosDevices.GroupBy(device => device.Mode))
			{
				var broker = _apnsServiceBrokerFactory.Create(deviceGroup.Key);

				DoWithBroker(
					broker,
					notification,
					() =>
					{
						foreach (var device in deviceGroup)
						{
							broker.QueueNotification(
								new ApnsNotification
								{
									DeviceToken = device.Token,
									Payload = CreateIosMessage(notification.Message)
								});
						}
					});
			}
		}

		private void DoWithBroker<TNotification>(
			IServiceBroker<TNotification> broker,
			Notification notification,
			Action action) where TNotification : INotification
		{
			try
			{
				broker.Start();

				broker.OnNotificationFailed += (message, aggregateEx) => aggregateEx.Handle(exception =>
				{
					NotificationFailed?.Invoke(this, new NotificationFailedEventArgs(aggregateEx));
					return true;
				});
				broker.OnNotificationSucceeded += gcmMessage =>
				{
					NotificationSuccess?.Invoke(this, new NotificationSuccessEventArgs(notification));
				};

				action();
			}
			catch (Exception e)
			{
				NotificationFailed?.Invoke(this, new NotificationFailedEventArgs(e));
			}
			finally
			{
				broker.Stop();
			}
		}

		private static JObject CreateIosMessage(string message)
		{
			return JObject.FromObject(
				new
				{
					type = EventType,
					aps = new
					{
						alert = message,
						sound = "default"
					}
				});
		}

		private static JObject CreateAndroidMessage(string message)
		{
			return JObject.FromObject(
				new
				{
					type = EventType,
					badge = 7,
					sound = "sound.caf",
					alert = message
				});
		}
	}
}