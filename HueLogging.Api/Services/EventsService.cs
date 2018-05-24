using HueLogging.Api.Models.Helpers;
using HueLogging.Api.Models.Helpers.Paging;
using HueLogging.Api.Models.Request.Filters;
using HueLogging.Api.Models.Response;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Services
{
    public class EventsService
    {
		private readonly IHueLoggingRepo hueLoggingRepo;
		
		public EventsService(IHueLoggingRepo hueLoggingRepo)
		{
			this.hueLoggingRepo = hueLoggingRepo;
		}

		public EventResponse GetRecentEvents(PagingRequest pagingRequest, TimeFrame timeFrame, string LightId)
		{
			var result = hueLoggingRepo.GetRecentEvents(LightId, TimeSpan.FromDays(timeFrame.DaysBack));
			var paging = new Paging(pagingRequest, result.AsQueryable().Count());
			result = result.GetPage(paging);
			var response = new EventResponse
			{
				Results = result,
				Page = paging.PagingResponse
			};
			return response;
		}
	}
}
