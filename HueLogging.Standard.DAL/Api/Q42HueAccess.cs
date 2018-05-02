using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
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

		private HueConfigStates hueConfig;

		IBridgeLocator _locator;
		ILocalHueClient _client;
		IHueLoggingRepo _hueLoggingRepo;

		public Q42HueAccess(IHueLoggingRepo hueLoggingRepo)
		{
			_locator = new HttpBridgeLocator();
			_hueLoggingRepo = hueLoggingRepo;
			hueConfig = hueLoggingRepo.GetRecentConfig();
		}

		public string GetBridgeIpForSetup()
		{
			IEnumerable<Q42.HueApi.Models.Bridge.LocatedBridge> bridgeIPs = _locator.LocateBridgesAsync(TimeSpan.FromSeconds(5)).Result;
			var ip = bridgeIPs.FirstOrDefault()?.IpAddress;
			_client = new LocalHueClient(ip);
			return ip;
		}

		public string GetAppKeyForSetup(string ip)
		{
			_client = new LocalHueClient(ip);
			return _client.RegisterAsync(APP_NAME, DEVICE_NAME).Result;
		}

		public HueConfigStates PersistConfig(HueConfigStates configInput)
		{
			var config = new HueConfigStates { IpAddress = configInput.IpAddress, Key = configInput.Key, AddDate = DateTime.UtcNow };
			_hueLoggingRepo.Save(config);
			return config;
		}

		public void Setup()
		{
			int retries = HueSetupOptions.MaxAttempts;
			if (hueConfig == null)
			{
				var ip = HueSetupOptions.Dns ?? GetBridgeIpForSetup();
				string appKey = String.Empty;
				while (string.IsNullOrEmpty(appKey) && retries >= 0)
				{
					Task.Delay(HueSetupOptions.WaitPeriodInMs).Wait();
					try
					{
						appKey = GetAppKeyForSetup(ip);
						retries--;
					}
					catch (Exception ex)
					{
						retries--;
					}

				}
				hueConfig = PersistConfig(new HueConfigStates
				{
					IpAddress = ip,
					Key = appKey
				});
			}
			_client = new LocalHueClient(hueConfig.IpAddress, hueConfig.Key);
		}

		public LightEvent GetLight(string id)
		{
			var light = _client.GetLightAsync(id).Result;
			return Convert(light);
		}

		public IEnumerable<LightEvent> GetLights()
		{
			var lights = _client.GetLightsAsync().Result;
			return lights.Select(Convert);
		}


		public bool HasBeenActiveSince(DateTime dateTime)
		{
			// TODO verify if this is UTC time or not
			var whitelists = _client.GetWhiteListAsync().Result;
			return whitelists.Where(x => x.Name != $"{APP_NAME}#{DEVICE_NAME}")
					.Any(x => DateTime.Parse(x.LastUsedDate) >= dateTime);
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
				State = new Models.State
				{
					Brightness = light.State.Brightness,
					Hue = (short)(light.State.Hue ?? 0),
					On = light.State.On,
					Reachable = light.State.IsReachable ?? false,
					Saturation = (short)(light.State.Saturation ?? 0)
				}
			};
		}
	}
}
