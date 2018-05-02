using HueLogging.Standard.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HueLogging.Web.Controllers
{
	public class FocusOnController : Controller
	{
		IHueLoggingManager _hueLoggingManager;
		IHueLoggingRepo _hueLoggingRepo;
		ILogger _logger;


		public FocusOnController(IHueLoggingManager hueLogger, IHueLoggingRepo hueLoggingRepo, ILogger<FocusOnController> logger)
		{
			_hueLoggingManager = hueLogger;
			_hueLoggingRepo = hueLoggingRepo;
			_logger = logger;
		}

		public IActionResult Index(string lightName, int daysBack = 30)
		{
			try
			{
				var light = _hueLoggingRepo.GetLightByName(lightName);
				if (light == null) return NotFound();
				ViewBag.DaysBack = daysBack;
				return View(light);
			} catch (Exception ex)
			{
				_logger.LogError(ex, "Error while attempting to fetch light details");
				return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
			}
			
		}
	}
}