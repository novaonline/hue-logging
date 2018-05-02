using HueLogging.Standard.Library.Helpers.Comparers;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace HueLogging.Standard.Library
{
	public class HueLoggingManager : IHueLoggingManager
	{
		private IHueLoggingRepo _hueLoggingRepo { get; set; }
		private IHueAccess _hueAccess { get; set; }

		private ILogger<HueLoggingManager> _logger;

		private LightEvent lastLightEvent;

		public HueLoggingManager(IHueLoggingRepo hueLoggingRepo, IHueAccess hueAccess, ILogger<HueLoggingManager> logger)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_hueAccess = hueAccess;
			_logger = logger;
			lastLightEvent = _hueLoggingRepo.GetLastEvent();

		}

		public bool HasPassedPreCondition()
		{
			var r = lastLightEvent != null ? _hueAccess.HasBeenActiveSince(lastLightEvent.AddDate) : true;
			var passFail = r ? "Passed" : "Failed";
			_logger.LogInformation($"Precondition {passFail}");
			return r;
		}

		public void LogEvent(bool shouldStartNewSession)
		{
			// loop through each light and determine which one was is different.
			_logger.LogInformation($"Looping through each light");
			var lights = _hueAccess.GetLights();
			foreach (var currentLight in lights)
			{
				_logger.LogInformation($"Light: {currentLight.Light.Id}");
				var lastRecordedEvent = _hueLoggingRepo.GetLastEvent(currentLight.Light.Id);
				if (new ActivityComparer().Compare(lastRecordedEvent, currentLight) != 0)
				{
					_logger.LogInformation($"Activity is different for {currentLight.Light.Id}");
					currentLight.AddDate = DateTime.UtcNow;
					_hueLoggingRepo.Save(currentLight);
					lastLightEvent = currentLight;
					_logger.LogInformation($"Saved {currentLight.Id} new info to database");
					if(new OnStateComparer().Compare(lastRecordedEvent, currentLight) != 0)
					{
						_logger.LogInformation($"State is different for {currentLight.Light.Id}");
						if (currentLight.State.On || shouldStartNewSession)
						{
							_logger.LogInformation($"Creating new session for {currentLight.Light.Id}");
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
								_logger.LogInformation($"Closed the session for {currentLight.Light.Id}");
							}
						}
					}
					
				}
			}
		}

		public void Start(bool shouldStartNewSession)
		{
			try
			{
				_hueAccess.Setup();
				if (HasPassedPreCondition())
				{
					LogEvent(shouldStartNewSession);
				}
			} catch (Exception ex)
			{
				_logger.LogError(ex, "Something happened while Logging Hue");
			}

		}
	}
}
