using System;
using PushSharp.Google;

namespace SadWave.Events.Api.Common.Notifications
{
	public class GcmServiceBrokerFactory
	{
		private readonly GcmConfiguration _configuration;

		public GcmServiceBrokerFactory(GcmConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public GcmServiceBroker Create()
		{
			return new GcmServiceBroker(_configuration);
		}
	}
}