using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace SadWave.Events.Api.Common.Vk
{
	public class VkClient
	{
		private readonly VkApi _api;
		private readonly VkSettings _settings;

		public VkClient(VkApi api, VkSettings settings)
		{
			_api = api ?? throw new ArgumentNullException(nameof(api));
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public Task<Group> GetEventAsync(string eventId)
		{
			if (eventId == null) throw new ArgumentNullException(nameof(eventId));

			return GetEventByIdAsync(eventId);
		}

		public Task<VkCollection<Photo>> GetPhotoAsync(long albumId, long ownerId)
		{
			if (albumId <= 0) throw new ArgumentOutOfRangeException(nameof(albumId));
			if (ownerId >= 0) throw new ArgumentOutOfRangeException(nameof(ownerId));

			return GetPhotoByIdAsync(albumId, ownerId);
		}

		private async Task<VkCollection<Photo>> GetPhotoByIdAsync(long albumId, long ownerId)
		{
			await AuthorizeIfItIsNotAsync();

			var parameters = new PhotoGetParams
			{
				OwnerId = ownerId,
				AlbumId = PhotoAlbumType.Profile,
				PhotoSizes = true
			};

			return await _api.Photo.GetAsync(parameters, true);
		}

		private async Task<Group> GetEventByIdAsync(string eventId)
		{
			await AuthorizeIfItIsNotAsync();
			var vkEventResult = await GetByIdAsync(null, eventId, GroupsFields.All, true);

			return vkEventResult.Single();
		}

		private async Task<ReadOnlyCollection<Group>> GetByIdAsync(IEnumerable<string> groupIds, string groupId, GroupsFields fields, bool skipAuthorization = false)
		{
			var result = await _api.CallAsync("groups.getById", new VkParameters
			{
				{
					"group_ids",
					groupIds
				},
				{
					"group_id",
					groupId
				},
				{
					nameof(fields),
					fields
				}
			}, skipAuthorization);

			return result.ToReadOnlyCollectionOf(x => (Group)x);
		}

		private async Task AuthorizeIfItIsNotAsync()
		{
			if (!_api.IsAuthorized)
			{
				await _api.AuthorizeAsync(new ApiAuthParams
				{
					AccessToken = _settings.AccessToken
				});
			}
		}
	}
}