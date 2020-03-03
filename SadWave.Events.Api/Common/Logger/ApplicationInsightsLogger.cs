using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace SadWave.Events.Api.Common.Logger
{
	public class ApplicationInsightsLogger : ILogger
	{
		private readonly TelemetryClient _telemetryClient;

		public ApplicationInsightsLogger()
		{
			_telemetryClient = new TelemetryClient();
		}

		public void Information(string message)
		{
			Track(message, SeverityLevel.Information);
		}

		public void Warning(string message)
		{
			Track(message, SeverityLevel.Warning);
		}

		public void Error(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

			DoWithFlush(() =>
				_telemetryClient.TrackException(new Exception(), new Dictionary<string, string> {{"Message", message}}));
		}

		public void Error(string message, Exception exception)
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));
			if (string.IsNullOrWhiteSpace(message))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

			DoWithFlush(() =>
				_telemetryClient.TrackException(exception, new Dictionary<string, string> {{"Message", message}}));
		}

		public void Error(Exception exception)
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			DoWithFlush(() => _telemetryClient.TrackException(exception));
		}

		public void Track(string message, SeverityLevel level)
		{
			if (string.IsNullOrWhiteSpace(message))
				return;

			DoWithFlush(() => _telemetryClient.TrackTrace(message, level));
		}

		private void DoWithFlush(Action action)
		{
			action();
			_telemetryClient.Flush();
		}
	}
}