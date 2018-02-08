using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HueLogging.Models.Interfaces;

namespace HueLogging.Web.Controllers
{
	public class FocusOnController : Controller
	{
		ILoggingManager _logger;
		IHueLoggingRepo _hueLoggingRepo;

		public FocusOnController(ILoggingManager logger, IHueLoggingRepo hueLoggingRepo)
		{
			_logger = logger;
			_hueLoggingRepo = hueLoggingRepo;
		}

		public IActionResult Index(string lightName, int daysBack = 30)
		{
			var lightId = _hueLoggingRepo.GetLightByName(lightName)?.Id;
			ViewBag.DaysBack = daysBack;
			var events = _hueLoggingRepo.GetRecentEvents(lightId, TimeSpan.FromDays(daysBack));
			return View(events);
		}
	}
}