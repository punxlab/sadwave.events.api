using System;
using PushSharp.Apple;

namespace SadWave.Events.Api.Common.Notifications
{
	public class ApnsServiceBrokerFactory
	{
		private readonly ApnsConfiguration _sandboxConfiguration;
		private readonly ApnsConfiguration _productionConfiguration;

		public ApnsServiceBrokerFactory(
			ApnsConfiguration productionConfiguration, ApnsConfiguration sandboxConfiguration)
		{
			_sandboxConfiguration = sandboxConfiguration ?? throw new ArgumentNullException(nameof(sandboxConfiguration));
			_productionConfiguration = productionConfiguration ?? throw new ArgumentNullException(nameof(productionConfiguration));
		}

		public ApnsServiceBroker Create(Mode mode)
		{
			switch (mode)
			{
				case Mode.Production:
					return new ApnsServiceBroker(_productionConfiguration);
				case Mode.Sandbox:
					return new ApnsServiceBroker(_sandboxConfiguration);
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown mode.");
			}
		}
	}
}