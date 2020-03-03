using System;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PushSharp.Apple;
using PushSharp.Google;
using SadWave.Events.Api.Common.Authentication;
using SadWave.Events.Api.Common.Events;
using SadWave.Events.Api.Common.Events.Parsers;
using SadWave.Events.Api.Common.Events.Providers;
using SadWave.Events.Api.Common.Facebook;
using SadWave.Events.Api.Common.Html;
using SadWave.Events.Api.Common.Logger;
using SadWave.Events.Api.Common.Notifications;
using SadWave.Events.Api.Common.Security;
using SadWave.Events.Api.Common.Storages;
using SadWave.Events.Api.Common.Vk;
using SadWave.Events.Api.Properties;
using SadWave.Events.Api.Repositories;
using SadWave.Events.Api.Repositories.Accounts;
using SadWave.Events.Api.Repositories.Cities;
using SadWave.Events.Api.Repositories.Devices;
using SadWave.Events.Api.Repositories.Events;
using SadWave.Events.Api.Repositories.Notifications;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Cities;
using SadWave.Events.Api.Services.Devices;
using SadWave.Events.Api.Services.Events;
using SadWave.Events.Api.Services.Notifications;
using VkNet;

namespace SadWave.Events.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var logger = new ApplicationInsightsLogger();

			try
			{
				var apnsProdConfiguration = new ApnsConfiguration(
					ApnsConfiguration.ApnsServerEnvironment.Production,
					Resources.SadwavePushCertificate,
					Configuration["Push:Ios:Certificate:Password"]);

				var apnsSandboxConfiguration = new ApnsConfiguration(
					ApnsConfiguration.ApnsServerEnvironment.Sandbox,
					Resources.SadwavePushCertificate,
					Configuration["Push:Ios:Certificate:Password"]);

				var gcmConfiguration = new GcmConfiguration(
					Configuration["Push:Android:Sender"],
					Configuration["Push:Android:AuthToken"],
					Configuration["Push:Android:PackageName"]);
				gcmConfiguration.GcmUrl = Configuration["Push:Android:Url"];

				var authenticationKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

				var issuer = Configuration["Jwt:Issuer"];

				var lifeTimeConfiguration = Configuration["Jwt:Lifetime"];
				TimeSpan? tokenLifeTime = null;
				if (lifeTimeConfiguration != null)
				{
					tokenLifeTime = TimeSpan.Parse(lifeTimeConfiguration);
				}

				var location = Assembly.GetEntryAssembly().Location;
				var rootDirectory = Path.GetDirectoryName(location);
				var databasePath = $"{rootDirectory}\\{Configuration["Database:Name"]}.sqlite";
				var context = new ConnectionFactory(databasePath);
				var initializer = new DatabaseInitializer(context);
				initializer.Initialize();

				services.AddSingleton<HtmlParser>();
				services.AddSingleton<HtmlProvider>();
				services.AddSingleton<ILogger>(logger);
				services.AddSingleton<VkApi>();
				services.AddSingleton<EventDetailsProviderFactory>();
				services.AddSingleton<SadWaveEventsParser>();
				services.AddSingleton<IEventsParser, EventsParser>();
				services.AddSingleton<FileCacheStorage>();
				services.AddSingleton<IPushNotifier, PushNotifier>();
				services.AddSingleton<INotificationsService, NotificationsService>();
				services.AddSingleton<IPasswordEncryptor, PasswordEncryptor>();
				services.AddSingleton<IEventsRepository, EventsRepository>();
				services.AddSingleton<IDevicesRepository, DevicesRepository>();
				services.AddSingleton<IAccountsRepository, AccountsRepository>();
				services.AddSingleton<ICitiesRepository, CitiesRepository>();
				services.AddSingleton<IEventsService, EventsService>();
				services.AddSingleton<IDevicesService, DevicesService>();
				services.AddSingleton<IAccountsService, AccountsService>();
				services.AddSingleton<ICitiesService, CitiesService>();
				services.AddSingleton<INotificationsRepository, NotificationsRepository>();
				services.AddSingleton(new FileCache(rootDirectory));
				services.AddSingleton(new ApnsServiceBrokerFactory(apnsProdConfiguration, apnsSandboxConfiguration));
				services.AddSingleton(new GcmServiceBrokerFactory(gcmConfiguration));
				services.AddSingleton(new AuthenticationOptions(authenticationKey, tokenLifeTime, issuer));
				services.AddSingleton(new FacebookSettings(
					Configuration["Facebook:AccessToken"],
					Configuration["Facebook:ApiUrl"],
					Configuration["Facebook:ApiVersion"]));
				services.AddSingleton(new VkSettings(Configuration["Vk:AccessToken"]));
				services.AddSingleton<IConnectionFactory<SqliteConnection>>(context);
				services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(
						options =>
						{
							options.TokenValidationParameters = new TokenValidationParameters
							{
								ValidateIssuer = true,
								ValidIssuer = issuer,
								ValidateLifetime = true,
								ValidateIssuerSigningKey = true,
								ValidateAudience = false,
								IssuerSigningKey = authenticationKey
							};
						});
				services.AddMvc();
			}
			catch (Exception e)
			{
				logger.Error(e);
				throw;
			}
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();
			app.UseMvc();
		}
	}
}