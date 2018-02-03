using HueLogging.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HueLogging.Models;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using System.Threading.Tasks;
using System.Linq;

namespace HueLogging.DAL.Api
{
	public class Q42HueAccess : IHueAccess
	{
		private const string APP_NAME = "HUE_LOGGING";
		private const string DEVICE_NAME = "DEVICE_NAME";

		IBridgeLocator _locator;
		ILocalHueClient _client;
		IHueLoggingRepo _hueLoggingRepo;

		public Q42HueAccess(IHueLoggingRepo hueLoggingRepo)
		{
			_locator = new HttpBridgeLocator();
			_hueLoggingRepo = hueLoggingRepo;
		}

		public void Setup()
		{
			int retries = 10;
			var config = _hueLoggingRepo.GetRecentConfig();
			if (config == null)
			{
				IEnumerable<Q42.HueApi.Models.Bridge.LocatedBridge> bridgeIPs = _locator.LocateBridgesAsync(TimeSpan.FromSeconds(5)).Result;
				var ip = bridgeIPs.FirstOrDefault()?.IpAddress;
				_client = new LocalHueClient(ip);
				string appKey = String.Empty;
				while (string.IsNullOrEmpty(appKey) && retries >= 0)
				{
					Task.Delay(1000).Wait();
					try
					{
						appKey = _client.RegisterAsync(APP_NAME, DEVICE_NAME).Result;
						retries--;
					}
					catch (Exception ex)
					{
						retries--;
					}

				}
				config = new HueConfigStates { IpAddress = ip, Key = appKey, AddDate = DateTime.UtcNow };
				_hueLoggingRepo.Save(config);
			}
			_client = new LocalHueClient(config.IpAddress, config.Key);
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
