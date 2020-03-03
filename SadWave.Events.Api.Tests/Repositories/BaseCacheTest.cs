using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using NUnit.Framework;
using SadWave.Events.Api.Common.Storages;

namespace SadWave.Events.Api.Tests.Repositories
{
	public abstract class BaseCacheTest
	{
		protected FileCacheStorage FileCacheStorage;

		[SetUp]
		public void Initialize()
		{
			var location = Assembly.GetEntryAssembly().Location;
			var rootDirectory = Path.GetDirectoryName(location);
			var nativeCache = new FileCache(rootDirectory);
			nativeCache.CleanCache();
			FileCacheStorage = new FileCacheStorage(nativeCache);
		}
	}
}