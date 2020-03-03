using System;

namespace SadWave.Events.Api.Services.Accounts
{
	public static class RoleUtils
	{
		public static string GetName(Role role)
		{
			if (TryGetName(role, out var name))
				return name;

			throw new ArgumentOutOfRangeException(nameof(role), role, "Incorrect role value.");
		}

		public static Role GetRole(string name)
		{
			if (TryGetRole(name, out var role))
				return role;

			throw new ArgumentOutOfRangeException(nameof(name), name, "Incorrect role name.");
		}

		public static bool TryGetName(Role role, out string roleName)
		{
			switch (role)
			{
				case Role.User:
					roleName = RoleName.User;
					return true;
				case Role.Admin:
					roleName = RoleName.Admin;
					return true;
				default:
					roleName = null;
					return false;
			}
		}

		public static bool TryGetRole(string name, out Role role)
		{
			switch (name)
			{
				case RoleName.User:
					role = Role.User;
					return true;
				case RoleName.Admin:
					role = Role.Admin;
					return true;
				default:
					role = Role.Unknown;
					return false;
			}
		}
	}
}