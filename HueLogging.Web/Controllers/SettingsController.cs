using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HueLogging.Web.Controllers
{
	public class SettingsController : Controller
	{
		private IHueLoggingRepo _hueLoggingRepo;
		private IHueAccess _hueAccess;
		private ILogger _logger;

		public SettingsController(IHueLoggingRepo hueLoggingRepo, IHueAccess hueAccess, ILogger<SettingsController> logger)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_hueAccess = hueAccess;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var m = _hueLoggingRepo.GetRecentConfig();
			if (m == null)
			{
				ViewData["Message"] = "You need to setup a connection to your hue.";
				return RedirectToAction("Setup");
			}
			return View(m);
		}

		public IActionResult Setup()
		{
			var options = new HueSetupOptions();
			return View(options);
		}

		[HttpPost]
		public IActionResult FindBridgeIP()
		{
			var ip = _hueAccess.GetBridgeIpForSetup();
			return Json(new { ip = ip });
		}

		[HttpPost]
		public IActionResult Authenticate(string ip)
		{
			string appKey = null;
			try
			{
				appKey = _hueAccess.GetAppKeyForSetup(ip);
				if (!string.IsNullOrEmpty(appKey))
				{
					_hueAccess.PersistConfig(new HueConfigStates
					{
						IpAddress = ip,
						Key = appKey
					});
					return Json(new { redirect = true, appKey = appKey });
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Did not Authenticate");
			}
			return Json(new { redirect = false, appKey = appKey });
		}
	}
}