using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLogging.Web.Models;
using Hangfire;
using HueLogging.Models.Interfaces;
using Hangfire.Storage;

namespace HueLogging.Web.Controllers
{
	public class HomeController : Controller
	{
		ILoggingManager _loggingManager;
		IHueLoggingRepo _hueLoggingRepo;

		public HomeController(ILoggingManager loggingManager, IHueLoggingRepo hueLoggingRepo)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_loggingManager = loggingManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GetSummary(int daysBack = 30)
		{
			var end = DateTime.UtcNow;
			var start = end.AddDays(-1 * daysBack);
			var m = _hueLoggingRepo.GetHueSessionSummary(start, end);
			return Json(new
			{
				datasets = new[] { new { data = m.Select(x => (int)Math.Round(x.TotalDuration.TotalMinutes)) } },
				labels = m.Select(x => x.Light.Name)
			});
		}

		[Route("/start")]
		public IActionResult Start()
		{
			RecurringJob.AddOrUpdate("HueLoggingJobId", () => Begin(), Cron.Minutely, TimeZoneInfo.Utc);
			return View();
		}

		[Route("/stop")]
		public IActionResult Stop()
		{
			// TODO: when gracefully shutting down, or on error, make session enddate utcnow
			RecurringJob.RemoveIfExists("HueLoggingJobId");
			return View();
		}

		public void Begin()
		{
			var shouldStartNewSession = true;
			using (var connection = JobStorage.Current.GetConnection())
			{
				var recurringJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == "HueLoggingJobId");

				if (recurringJob != null && recurringJob.LastExecution.HasValue)
				{
					var lastJob = connection.GetJobData((int.Parse(recurringJob.LastJobId) - 1).ToString());
					if (lastJob != null)
					{
						shouldStartNewSession = (DateTime.UtcNow - lastJob.CreatedAt).TotalMinutes >= 2;
					}
				}
			}
			_loggingManager.Start(shouldStartNewSession);
		}


		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
