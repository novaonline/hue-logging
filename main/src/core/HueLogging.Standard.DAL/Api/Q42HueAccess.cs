using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Standard.DAL.Api
{
	public class Q42HueAccess : IHueAccess
	{
		private const string APP_NAME = "HUE_LOGGING";
		private const string DEVICE_NAME = "DEVICE_NAME";
		readonly IBridgeLocator _locator;
		ILocalHueClient _client;
		readonly ILogger<Q42HueAccess> _logger;

		readonly string ipAddress;
		string key;

		public Q42HueAccess(IConfiguration hueLoggingConfig, ILogger<Q42HueAccess> logger)
		{
			_logger = logger;
			_locator = new HttpBridgeLocator();
			ipAddress = hueLoggingConfig["HueLogging:IpAddress"];
			key = hueLoggingConfig["HueLogging:ApiKey"];
		}

		public async Task<string> GetBridgeIpForSetup()
		{
			IEnumerable<Q42.HueApi.Models.Bridge.LocatedBridge> bridgeIPs = await _locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
			var ip = bridgeIPs.FirstOrDefault()?.IpAddress;
			_client = new LocalHueClient(ip);
			return ip;
		}

		public async Task<string> GetAppKeyForSetup(string ip)
		{
			_client = new LocalHueClient(ip);
			return await _client.RegisterAsync(APP_NAME, DEVICE_NAME);
		}


		public async Task<(string IpAddress, string Key)> Setup()
		{
			int retries = HueSetupOptions.MaxAttempts;
			var ip = ipAddress;
			if (key == null)
			{
				ip = ip ?? await GetBridgeIpForSetup();
				string appKey = String.Empty;
				_logger.LogInformation("Please click on the hue button.");

				while (string.IsNullOrEmpty(key) && retries >= 0)
				{
					Task.Delay(HueSetupOptions.WaitPeriodInMs).Wait();
					try
					{
						key = await GetAppKeyForSetup(ip);
						retries--;
					}
					catch (Exception)
					{
						retries--;
						_logger.LogInformation($"Waiting... patience level down to {retries}");
					}

				}
			}
			_client = new LocalHueClient(ip, key);
			return (ip, key);
		}

		public async Task<LightEvent> GetLight(string id)
		{
			var light = await _client.GetLightAsync(id);
			return Convert(light);
		}

		public async Task<IEnumerable<LightEvent>> GetLights()
		{
			var lights = _client.GetLightsAsync();
			return (await lights).Select(Convert);
		}


		public async Task<bool> HasBeenActiveSince(DateTimeOffset dateTime)
		{
			var whitelists = await _client.GetWhiteListAsync();
			var appMeta = whitelists
				.Where(x => x.Name != $"{APP_NAME}#{DEVICE_NAME}" && x.LastUsedDate.HasValue)
				.OrderByDescending(x => x.LastUsedDate)
				.ToList();

			DateTime recentDate = appMeta.FirstOrDefault()?.LastUsedDate ?? DateTime.Now;
			var r = recentDate >= dateTime.DateTime;
			return r;
		}

		private LightEvent Convert(Q42.HueApi.Light light)
		{
			return new LightEvent
			{
				Light = new Models.Light
				{
					Id = light.Id,
					Name = light.Name,
					HueType = light.Type,
					ModelId = light.ModelId,
					SWVersion = light.SoftwareVersion
				},
				State = new LightState
				{
					Brightness = light.State.Brightness,
					Hue = (short)(light.State.Hue ?? 0),
					On = light.State.On,
					Reachable = light.State.IsReachable ?? false,
					Saturation = (short)(light.State.Saturation ?? 0),
					AddDate = DateTimeOffset.Now
				}
			};
		}
	}
}
