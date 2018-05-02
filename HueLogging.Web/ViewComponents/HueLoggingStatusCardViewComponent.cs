using Hangfire;
using Hangfire.Storage;
using HueLogging.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Web.ViewComponents
{
	public class HueLoggingStatusCardViewComponent : ViewComponent
	{
		public HueLoggingStatusCardViewComponent() : base()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync(string bootstrapCol)
		{
			var m = new StatusCardViewModel
			{
				BootstrapColumnClasses = bootstrapCol
			};

			string lastRunResult = string.Empty;
			using (var connection = JobStorage.Current.GetConnection())
			{
				var recurringJobs = connection.GetRecurringJobs();
				var job = await Task.Run(() =>
				recurringJobs.FirstOrDefault(p =>
					p.Id.Equals(Controllers.HomeController.JOBID, StringComparison.InvariantCultureIgnoreCase)));
				if (job != null)
				{
					try
					{
						var jobState = connection.GetStateData(job.LastJobId);
						lastRunResult = jobState.Name; // For Example: "Succeeded", "Processing", "Deleted"
						switch (lastRunResult.ToLower())
						{

							case "processing":
							case "succeeded":
								m.Status = HueLoggingStatus.RUNNING;
								break;
							case "deleted":
								m.Status = HueLoggingStatus.STOPPED;
								break;
							default:
								m.Status = HueLoggingStatus.UNKNOWN;
								break;
						}
					}
					catch (Exception ex)
					{
						// TODO: log
						m.Status = HueLoggingStatus.STOPPED;
					}
				}
			}
			return View(m);
		}
	}
}
