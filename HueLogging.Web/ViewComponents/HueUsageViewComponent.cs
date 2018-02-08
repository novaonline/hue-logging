using HueLogging.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Web.ViewComponents
{
	[ViewComponent(Name = "HueUsage")]
	public class HueUsageViewComponent : ViewComponent
	{
		IHueLoggingRepo _hueLoggingRepo;
		public HueUsageViewComponent(IHueLoggingRepo hueLoggingRepo) : base()
		{
			_hueLoggingRepo = hueLoggingRepo;
		}

		public async Task<IViewComponentResult> InvokeAsync(string lightId, int daysBack = 30)
		{
			ViewBag.DaysBack = daysBack;
			var start = DateTime.UtcNow.AddDays(-1 * daysBack); 
			var end = DateTime.UtcNow;
			var usage = await Task.Run(() => _hueLoggingRepo.GetHueSessions(start, end).Where(x => x.Light.Id == lightId));
			return View(usage);
		}
	}
}
