using System;
using SadWave.Events.Api.Common.Facebook;
using SadWave.Events.Api.Common.Vk;
using VkNet;

namespace SadWave.Events.Api.Common.Events.Providers
{
	public class EventDetailsProviderFactory
	{
		private readonly FacebookSettings _facebookSettings;
		private readonly VkSettings _vkSettings;

		public EventDetailsProviderFactory(
			FacebookSettings facebookSettings,
			VkSettings vkSettings)
		{
			_facebookSettings = facebookSettings ?? throw new ArgumentNullException(nameof(facebookSettings));
			_vkSettings = vkSettings ?? throw new ArgumentNullException(nameof(vkSettings));
		}

		public IEventDetailsProvider Create(Uri uri)
		{
			if (uri == null)
				return null;

			if(uri.Host.Contains(VkEventProvider.HostName))
				return new VkEventProvider(new VkClient(new VkApi(), _vkSettings));

			if(uri.Host.Contains(FacebookEventProvider.HostName))
				return new FacebookEventProvider(new FacebookClient(_facebookSettings));

			return null;
		}
	}
}