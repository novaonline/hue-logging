using HueLogging.Standard.Library.Helpers.Comparers;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Standard.Library
{
	public class HueLoggingManager : IHueLoggingManager
	{
		private readonly IHueLoggingStateManager _stateManager;
		private readonly IHueLoggingSink<LightEvent> _writer;
		private readonly IHueAccess _hueAccess;
		private readonly ILogger<HueLoggingManager> _logger;

		private readonly ActivityComparer _activityComparer;

		public HueLoggingManager(ActivityComparer activityComparer,
			IHueLoggingStateManager stateManager,
			IHueLoggingSink<LightEvent> writer,
			IHueAccess hueAccess,
			ILogger<HueLoggingManager> logger)
		{
			_stateManager = stateManager;
			_writer = writer;
			_hueAccess = hueAccess;
			_logger = logger;
			_activityComparer = activityComparer;

		}

		public async Task<bool> HasPassedPreCondition()
		{
			var lastLightEvent = await _stateManager.GetLastEvent();
			var r = lastLightEvent != null ? await _hueAccess.HasBeenActiveSince(lastLightEvent.AddDate) : true;
			var passFail = r ? "Passed" : "Failed";
			_logger.LogInformation($"Precondition {passFail}");
			return r;
		}

		public async Task LogEvent(bool shouldStartNewSession)
		{
			// loop through each light and determine which one was is different.
			_logger.LogInformation($"Looping through each light");
			var lights = await _hueAccess.GetLights();
			foreach (var currentLight in lights)
			{
				var count = lights.Count();
				_logger.LogInformation($"Light: {currentLight.Light.Id}");
				var lastRecordedEvent = await _stateManager.GetLastEvent(currentLight.Light.Id);
				if (_activityComparer.Compare(lastRecordedEvent, currentLight) != 0)
				{
					_logger.LogInformation($"Activity is different for {currentLight.Light.Id}");
					currentLight.AddDate = DateTime.UtcNow;
					await _writer.Save(currentLight);
					await _stateManager.SaveAsLastLightEvent(currentLight);
					_logger.LogInformation($"Wrote {currentLight.Id} to writer");
				}
			}
		}

		public async Task Start(bool shouldStartNewSession)
		{
			try
			{
				await _hueAccess.Setup();
				var precon = await HasPassedPreCondition();
				if (precon)
				{
					await LogEvent(shouldStartNewSession);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Something happened while Logging Hue");
			}

		}
	}
}
