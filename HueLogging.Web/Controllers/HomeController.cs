using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLogging.Web.Models;
using Hangfire;
using HueLogging.Models.Interfaces;

namespace HueLogging.Web.Controllers
{
	public class HomeController : Controller
	{
		ILoggingManager _loggingManager;

		public HomeController(ILoggingManager loggingManager)
		{
			_loggingManager = loggingManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Start()
		{
			RecurringJob.AddOrUpdate("HueLoggingJobId", () => _loggingManager.Start(), Cron.Minutely, TimeZoneInfo.Utc, "HueLogging");
			return View();
		}

		public IActionResult Stop()
		{
			RecurringJob.RemoveIfExists("HueLoggingJobId");
			return View();
		}


		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
