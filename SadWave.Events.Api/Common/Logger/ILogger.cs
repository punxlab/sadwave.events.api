using System;

namespace SadWave.Events.Api.Common.Logger
{
	public interface ILogger
	{
		void Information(string message);

		void Warning(string message);

		void Error(string message);

		void Error(Exception exception);

		void Error(string message, Exception exception);
	}
}