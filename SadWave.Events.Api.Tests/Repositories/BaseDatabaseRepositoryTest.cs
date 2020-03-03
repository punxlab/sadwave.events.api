using System.IO;
using NUnit.Framework;
using SadWave.Events.Api.Repositories;

namespace SadWave.Events.Api.Tests.Repositories
{
	public abstract class BaseDatabaseRepositoryTest
	{
		private readonly string _databaseName;
		protected ConnectionFactory ConnectionTestory;

		protected BaseDatabaseRepositoryTest(string databaseName)
		{
			_databaseName = databaseName;
		}

		[SetUp]
		public void Initialize()
		{
			if (File.Exists(_databaseName))
			{
				File.Delete(_databaseName);
			}

			ConnectionTestory = new ConnectionFactory(_databaseName);
			var dataBaseInitializer = new DatabaseInitializer(ConnectionTestory);
			dataBaseInitializer.InitializeSchema();
			dataBaseInitializer.InitializeDictionaries();
		}
	}
}
