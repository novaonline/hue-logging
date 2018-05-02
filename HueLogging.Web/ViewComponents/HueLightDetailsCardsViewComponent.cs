using HueLogging.Standard.Models.Interfaces;
using HueLogging.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Web.ViewComponents
{
	public class HueLightDetailsCardsViewComponent : ViewComponent
	{
		IHueLoggingRepo _hueLoggingRepo;
		public HueLightDetailsCardsViewComponent(IHueLoggingRepo hueLoggingRepo) : base()
		{
			_hueLoggingRepo = hueLoggingRepo;
		}

		public async Task<IViewComponentResult> InvokeAsync(int daysBack = 30, string bootstrapCol = "")
		{
			var m = new HueLightDetailsCardViewModel()
			{
				BootstrapColumnClasses = bootstrapCol
			};

			var lights = await Task.Run(() =>
			{
				var sDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));
				var eDate = DateTime.UtcNow;
				return _hueLoggingRepo.GetHueSessions(sDate, eDate).Select(x => x.Light);
			});
			m.Lights = lights;
			return View(m);
		}
	}
}
