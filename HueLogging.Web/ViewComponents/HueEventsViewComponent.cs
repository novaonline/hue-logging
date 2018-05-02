using HueLogging.Standard.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HueLogging.Web.ViewComponents
{
	[ViewComponent(Name = "HueEvents")]
    public class HueEventsViewComponent : ViewComponent
    {
		IHueLoggingRepo _hueLoggingRepo;
		public HueEventsViewComponent(IHueLoggingRepo hueLoggingRepo) : base()
		{
			_hueLoggingRepo = hueLoggingRepo;
		}

		public async Task<IViewComponentResult> InvokeAsync(string lightId, int daysBack = 30)
		{
			ViewBag.DaysBack = daysBack;
			var e = await Task.Run(() => _hueLoggingRepo.GetRecentEvents(lightId, TimeSpan.FromDays(daysBack)));
			return View(e);
		}
	}
}
