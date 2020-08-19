using System;
using Dapper;

namespace SadWave.Events.Api.Repositories
{
	public class DatabaseInitializer
	{
		private readonly ConnectionFactory _connectionFactory;

		public DatabaseInitializer(ConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public void Initialize()
		{
			InitializeSchema();
			InitializeDictionaries();
			InitializeData();
		}

		public void InitializeSchema()
		{
			using (var connection = _connectionFactory.CreateConnection())
			{

				connection.Execute(
@"
BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS `Roles` (
	`Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	`Name` nvarchar ( 50 ) NOT NULL COLLATE NOCASE
);

CREATE TABLE IF NOT EXISTS `DeviceOs` (
	`Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	`Os` varchar ( 50 ) NOT NULL COLLATE NOCASE
);

CREATE TABLE IF NOT EXISTS `Devices` (
	`Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	`DeviceOsId` integer NOT NULL,
	`DeviceToken` nvarchar ( 450 ) NOT NULL COLLATE NOCASE,
	`Sandbox` bit NOT NULL,
	`CityId` integer,
	FOREIGN KEY(`CityId`) REFERENCES `Cities`(`Id`),
	FOREIGN KEY(`DeviceOsId`) REFERENCES `DeviceOs`(`Id`)
);

CREATE TABLE IF NOT EXISTS `Cities` (
	`Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	`Alias` nvarchar ( 10 ) NOT NULL COLLATE NOCASE,
	`Name` nvarchar ( 100 ) NOT NULL COLLATE NOCASE,
	`Uri` nvarchar NOT NULL COLLATE NOCASE
);

CREATE TABLE IF NOT EXISTS `Accounts` (
	`Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	`Login` nvarchar ( 50 ) NOT NULL COLLATE NOCASE,
	`Password` blob ( 64 ) NOT NULL,
	`RoleId` integer NOT NULL,
	FOREIGN KEY(`RoleId`) REFERENCES `Roles`(`Id`)
);

CREATE TABLE IF NOT EXISTS `EventsPhotos` (
	`EventUrl` varchar ( 255 ) NOT NULL PRIMARY KEY,
	`PhotoUrl` varchar ( 255 ),
	`PhotoWidth` integer NOT NULL,
	`PhotoHeight` integer NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS `Roles_UX_Roles_Name` ON `Roles` (
	`Name` DESC
);
CREATE UNIQUE INDEX IF NOT EXISTS `Cities_UX_Cities_Uri` ON `Cities` (
	`Uri` DESC
);
CREATE UNIQUE INDEX IF NOT EXISTS `Devices_UX_Devices_DeviceToken` ON `Devices` (
	`DeviceToken` DESC
);
CREATE UNIQUE INDEX IF NOT EXISTS `DeviceOs_UX_DeviceOs_Os` ON `DeviceOs` (
	`Os` DESC
);
CREATE UNIQUE INDEX IF NOT EXISTS `Cities_UX_Cities_Alias` ON `Cities` (
	`Alias` DESC
);
CREATE UNIQUE INDEX IF NOT EXISTS `Accounts_UX_Accounts_Login` ON `Accounts` (
	`Login` DESC
);

COMMIT;
");
			}
		}

		public void InitializeDictionaries()
		{
			using (var connection = _connectionFactory.CreateConnection())
			{
				connection.Execute(
					@"
BEGIN TRANSACTION;

INSERT OR IGNORE INTO `Roles` (Name)
VALUES
	('admin'),
	('user');

INSERT OR IGNORE INTO `DeviceOs` (Os)
VALUES
	('ios'),
	('android');

COMMIT;
");
			}
		}

		public void InitializeData()
		{
			using (var connection = _connectionFactory.CreateConnection())
			{
				connection.Execute(
@"
BEGIN TRANSACTION;

INSERT OR IGNORE INTO `Cities` (Alias, Name, Uri)
VALUES
	('msk','Москва','http://sadwave.com/afisha-moskva/'),
	('spb','Санкт-Петербург','http://sadwave.com/events-spb/'),
	('saratov','Саратов','http://sadwave.com/afisha-saratov/'),
	('kiev','Киев','http://sadwave.com/events/afisha-kiev/');

INSERT OR IGNORE INTO `Accounts` (Login, Password, RoleId)
VALUES
	('sadwave-admin',X'56fe54fe5a070278bc6e3bb697bd52dd232fd5cfbdda3ccba43369c9c910754a9e7ce79d00000000000000000000000000000000000000000000000000000000',1);

COMMIT;
");
			}

		}
	}
}
