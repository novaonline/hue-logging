using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLogging.Models.Interfaces;
using HueLogging.Models;

namespace HueLogging.Web.Controllers
{
	public class SettingsController : Controller
	{
		private IHueLoggingRepo _hueLoggingRepo;
		private IHueAccess _hueAccess;

		public SettingsController(IHueLoggingRepo hueLoggingRepo, IHueAccess hueAccess)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_hueAccess = hueAccess;
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
				// was not authenticated
			}
			return Json(new { redirect = false, appKey = appKey });
		}
	}
}