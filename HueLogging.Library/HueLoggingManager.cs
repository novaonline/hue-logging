using System;
using HueLogging.Library.Helpers.Comparers;
using HueLogging.Models;
using HueLogging.Models.Interfaces;
using System.Threading.Tasks;

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
			return lastLightEvent != null ? _hueAccess.HasBeenActiveSince(lastLightEvent.AddDate) : true;
		}

		public void LogEvent(bool shouldStartNewSession)
		{
			// loop through each life and determine which one was is different.
			var lights = _hueAccess.GetLights();
			foreach (var currentLight in lights)
			{
				var lastRecordedEvent = _hueLoggingRepo.GetLastEvent(currentLight.Light.Id);
				if (new ActivityComparer().Compare(lastRecordedEvent, currentLight) != 0)
				{
					currentLight.AddDate = DateTime.UtcNow;
					_hueLoggingRepo.Save(currentLight);
					if(new OnStateComparer().Compare(lastRecordedEvent, currentLight) != 0)
					{
						if (currentLight.State.On || shouldStartNewSession)
						{
							_hueLoggingRepo.Save(new HueSession
							{
								StartDate = DateTime.UtcNow,
								LightId = currentLight.Light.Id
							});
						}
						else
						{
							var lastSession = _hueLoggingRepo.GetLastIncompleteSession(currentLight.Light.Id);
							if (lastSession != null)
							{
								lastSession.EndDate = DateTime.UtcNow;
								_hueLoggingRepo.Save(lastSession);
							}
						}
					}
					
				}
			}
		}

		public void Start(bool shouldStartNewSession)
		{
			_hueAccess.Setup();
			if (HasPassedPreCondition())
			{
				LogEvent(shouldStartNewSession);
			}
		}
	}
}
