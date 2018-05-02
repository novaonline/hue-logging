using Hangfire;
using Hangfire.Storage;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace HueLogging.Web.Controllers
{
	public class HomeController : Controller
	{
		public static readonly string JOBID = "HueLoggingJobId";
		IHueLoggingManager _loggingManager;
		IHueLoggingRepo _hueLoggingRepo;
		ILogger _logger;

		public HomeController(IHueLoggingManager loggingManager, IHueLoggingRepo hueLoggingRepo, ILogger<HomeController> logger)
		{
			_hueLoggingRepo = hueLoggingRepo;
			_loggingManager = loggingManager;
			_logger = logger;
		}

		public IActionResult Index(int daysBack = 30)
		{
			ViewBag.DaysBack = daysBack;
			ViewBag.Newbie = _hueLoggingRepo.GetRecentConfig() == null ? true : false;
			return View();
		}

		[HttpPost]
		public IActionResult GetSummary(int daysBack = 30)
		{
			try
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
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Unable to Get Summary");
				return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
			}
		}

		[Route("/start")]
		public IActionResult Start()
		{
			RecurringJob.AddOrUpdate(JOBID, () => GetLastJobAndBegin(), Cron.Minutely, TimeZoneInfo.Utc);
			return View("SimpleMessage", new SimpleMessageViewModel
			{
				Message = "Hue Logging Starting",
				Title = "Hue Log Activation"
			});
		}

		[Route("/stop")]
		public IActionResult Stop()
		{
			// TODO: when gracefully shutting down, or on error, make session enddate utcnow
			RecurringJob.RemoveIfExists(JOBID);
			return View("SimpleMessage", new SimpleMessageViewModel
			{
				Message = "Hue Logging Stopping",
				Title = "Hue Log Deactivation"
			});
		}

		public void GetLastJobAndBegin()
		{
			var shouldStartNewSession = true;
			using (var connection = JobStorage.Current.GetConnection())
			{
				var recurringJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == JOBID);

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
