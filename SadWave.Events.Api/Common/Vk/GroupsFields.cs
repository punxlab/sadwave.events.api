using VkNet.Enums.Filters;

namespace SadWave.Events.Api.Common.Vk
{
	public sealed class GroupsFields : MultivaluedFilter<GroupsFields>
	{
		public static readonly GroupsFields CityId = RegisterPossibleValue(1UL, "city");
		public static readonly GroupsFields CountryId = RegisterPossibleValue(2UL, "country");
		public static readonly GroupsFields Place = RegisterPossibleValue(4UL, "place");
		public static readonly GroupsFields Description = RegisterPossibleValue(8UL, "description");
		public static readonly GroupsFields WikiPage = RegisterPossibleValue(16UL, "wiki_page");
		public static readonly GroupsFields MembersCount = RegisterPossibleValue(32UL, "members_count");
		public static readonly GroupsFields Counters = RegisterPossibleValue(64UL, "counters");
		public static readonly GroupsFields StartDate = RegisterPossibleValue(128UL, "start_date");
		public static readonly GroupsFields EndDate = RegisterPossibleValue(256UL, "finish_date");
		public static readonly GroupsFields CanPost = RegisterPossibleValue(512UL, "can_post");
		public static readonly GroupsFields CanSeelAllPosts = RegisterPossibleValue(1024UL, "can_see_all_posts");
		public static readonly GroupsFields MainAlbumId = RegisterPossibleValue(2048UL, "main_album_id");
		public static readonly GroupsFields CanCreateTopic = RegisterPossibleValue(4096UL, "can_create_topic");
		public static readonly GroupsFields Activity = RegisterPossibleValue(8192UL, "activity");
		public static readonly GroupsFields Status = RegisterPossibleValue(16384UL, "status");
		public static readonly GroupsFields Contacts = RegisterPossibleValue(32768UL, "contacts");
		public static readonly GroupsFields Links = RegisterPossibleValue(65536UL, "links");
		public static readonly GroupsFields FixedPostId = RegisterPossibleValue(131072UL, "fixed_post");
		public static readonly GroupsFields IsVerified = RegisterPossibleValue(262144UL, "verified");
		public static readonly GroupsFields Site = RegisterPossibleValue(524288UL, "site");
		public static readonly GroupsFields BanInfo = RegisterPossibleValue(1048576UL, "ban_info");

		public static readonly GroupsFields All = CityId | CountryId | Place | Description | WikiPage | MembersCount | Counters | StartDate | EndDate | CanPost | CanSeelAllPosts | CanCreateTopic | Activity | Status | Contacts | Links | FixedPostId | IsVerified | Site | BanInfo | MainAlbumId;
	}
}