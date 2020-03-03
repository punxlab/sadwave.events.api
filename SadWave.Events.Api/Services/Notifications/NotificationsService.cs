using System;
using System.Threading.Tasks;
using PushSharp.Apple;
using PushSharp.Common;
using PushSharp.Google;
using SadWave.Events.Api.Common.Notifications;
using SadWave.Events.Api.Repositories.Notifications;
using SadWave.Events.Api.Services.Devices;
using ILogger = SadWave.Events.Api.Common.Logger.ILogger;

namespace SadWave.Events.Api.Services.Notifications
{
	public class NotificationsService : INotificationsService
	{
		private readonly IPushNotifier _notifier;
		private readonly IDevicesService _service;
		private readonly INotificationsRepository _notificationsRepository;
		private readonly ILogger _logger;

		public NotificationsService(
			IPushNotifier notifier,
			IDevicesService service,
			INotificationsRepository notificationsRepository,
			ILogger logger)
		{
			_notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_notificationsRepository = notificationsRepository ?? throw new ArgumentNullException(nameof(notificationsRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_notifier.NotificationFailed += async (s, e) => await OnNotificationFailed(e);
			_notifier.NotificationSuccess += (s, e) => OnNotificationSuccess(e);
		}

		public void Notify(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			_notifier.Notify(notification);
		}

		public bool WasCityNotifiedToday(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			var notificationDate = _notificationsRepository.GetDate(cityAlias);
			return notificationDate.Date >= DateTime.UtcNow.Date;
		}

		public void MarkCityAsNotified(string cityAlias)
		{
			if (string.IsNullOrWhiteSpace(cityAlias))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(cityAlias));

			_notificationsRepository.SetDate(cityAlias, DateTime.UtcNow.Date);
		}

		public void DeleteLastNotificationDate(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(city));

			_notificationsRepository.DeleteDate(city);
		}

		public DateTime GetLastNotificationDate(string city)
		{
			if (string.IsNullOrWhiteSpace(city))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(city));

			return _notificationsRepository.GetDate(city);
		}

		private async Task OnNotificationFailed(NotificationFailedEventArgs notificationFailedEventArgs)
		{
			var exception = notificationFailedEventArgs.Exception;
			if (exception is ApnsNotificationException apnsNotificationException)
			{
				_logger.Error(
					$"Apple Notification Failed: ID={apnsNotificationException.Notification.Identifier}, Code={apnsNotificationException.ErrorStatusCode}", apnsNotificationException);
			}
			else if (exception is GcmNotificationException gcmNotificationException)
			{
				_logger.Error($"GCM Notification Failed: ID={gcmNotificationException.Notification}, Desc={gcmNotificationException.Description}", gcmNotificationException);
			}
			else if (exception is GcmMulticastResultException multicastException)
			{
				foreach (var succeededNotification in multicastException.Succeeded)
				{
					_logger.Information($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
				}

				foreach (var failedKvp in multicastException.Failed)
				{
					_logger.Error($"GCM Notification Failed: ID={failedKvp.Key.MessageId}, Exception={failedKvp.Value}", multicastException);
				}
			}
			else if (exception is DeviceSubscriptionExpiredException expiredException)
			{
				var oldId = expiredException.OldSubscriptionId;
				var newId = expiredException.NewSubscriptionId;

				_logger.Warning($"Device RegistrationId Expired: {oldId}");

				if (!string.IsNullOrWhiteSpace(newId))
				{
					_logger.Warning($"Device RegistrationId Changed To: {newId}");
				}

				await DeviceSubscriptionExpired(expiredException.OldSubscriptionId, expiredException.NewSubscriptionId);
			}
			else
			{
				_logger.Error($"Apple Notification Failed for some unknown reason : {exception.InnerException}");
			}
		}

		private void OnNotificationSuccess(NotificationSuccessEventArgs e)
		{
			_logger.Information($"Notification for {e.Notification.CityAlias} was sent successfully. Type of notification: {e.Notification.GetType()}.");
		}

		private async Task DeviceSubscriptionExpired(string oldId, string newId)
		{
			if (!string.IsNullOrWhiteSpace(oldId))
			{
				if (!string.IsNullOrWhiteSpace(newId))
				{
					await _service.UpdateDeviceAsync(oldId, newId);
				}
				else
				{
					await _service.DeleteAsync(oldId);
				}
			}
		}
	}
}
