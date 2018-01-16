using System;
using HueLogging.DAL.Api;
using HueLogging.DAL.Repository;
using HueLogging.Library.Helpers.Comparers;
using HueLogging.Models;

namespace HueLogging.Library
{
	public class HueLoggingManager : ILoggingManager
	{
		private IHueLoggingRepo _hueLoggingRepo { get; set; }
		private IHueAccess _hueAccess { get; set; }

		public HueLoggingManager(IHueLoggingRepo hueLoggingRepo, IHueAccess hueAccess)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_hueAccess = hueAccess;
		}

		public bool HasPassedPreCondition()
		{
			var lastLightEvent = _hueLoggingRepo.GetLastEvent();
			return lastLightEvent != null ? _hueAccess.HasBeenActiveSince(lastLightEvent.AddDate) : false;
		}

		public void LogEvent()
		{
			// loop through each life and determine which one was is different.
			var lights = _hueAccess.GetLights();
			foreach (var currentLight in lights)
			{
				var lastRecordedEvent = _hueLoggingRepo.GetLastEvent(currentLight.Id);
				if (new ActivityComparer().Compare(lastRecordedEvent?.Light, currentLight) != 0)
				{
					_hueLoggingRepo.Save(new LightEvent
					{
						Light = currentLight,
						AddDate = DateTime.UtcNow
					});
				}
			}
		}

		public void Start()
		{
			if (HasPassedPreCondition())
			{
				LogEvent();
			}
		}
	}
}
