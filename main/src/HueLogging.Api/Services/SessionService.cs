using HueLogging.Api.Models.Helpers;
using HueLogging.Api.Models.Helpers.Paging;
using HueLogging.Api.Models.Request;
using HueLogging.Api.Models.Request.Filters;
using HueLogging.Api.Models.Response;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Services
{
	public class SessionService
	{
		private readonly IHueLoggingWriter hueLoggingRepo;

		public SessionService(IHueLoggingWriter hueLoggingRepo)
		{
			this.hueLoggingRepo = hueLoggingRepo;
		}

		public SessionSummaryResponse GetRecentSessionsSummary(PagingRequest pagingRequest, TimeFrame timeFrame, Func<HueSessionSummary, bool> whereFilter = null)
		{
			//var sessions = hueLoggingRepo.GetHueSessionSummary(DateTime.UtcNow.AddDays(-1 * timeFrame.DaysBack), DateTime.UtcNow);
			//if (whereFilter != null)
			//{
			//	sessions = sessions.Where(whereFilter);
			//}
			//var paging = new Paging(pagingRequest, sessions.AsQueryable().Count());
			//sessions = sessions.GetPage(paging);
			//var response = new SessionSummaryResponse
			//{
			//	Results = sessions,
			//	Page = paging.PagingResponse
			//};
			//return response;
			return new SessionSummaryResponse();
		}

		public SessionSummaryResponse GetSessionsSummaryByFilter(PagingRequest pagingRequest, SessionFilter sessionFilter)
		{
			// Luckily we only have 1 filter so this pretty much implies its a filter by lightName
			var response = GetRecentSessionsSummary(pagingRequest, new TimeFrame { DaysBack = 365 }, x => x.Light.Name == sessionFilter.LightName); // get all
			response.Results = response.Results;
			return response;
		}

		public SessionResponse GetSessions(PagingRequest pagingRequest, TimeFrame timeFrame, Func<HueSession,bool> whereFilter = null)
		{
			//var sessions = hueLoggingRepo.GetHueSessions(DateTime.UtcNow.AddDays(-1 * timeFrame.DaysBack), DateTime.UtcNow);
			//if(whereFilter != null)
			//{
			//	sessions = sessions.Where(whereFilter);
			//}
			//var paging = new Paging(pagingRequest, sessions.AsQueryable().Count());
			//sessions = sessions.GetPage(paging);
			//var response = new SessionResponse
			//{
			//	Results = sessions,
			//	Page = paging.PagingResponse
			//};
			//return response;
			return new SessionResponse();
		}

		public SessionResponse GetSessionsByFilter(PagingRequest pagingRequest, SessionFilter sessionFilter)
		{
			var response = GetSessions(pagingRequest, new TimeFrame { DaysBack = 365 }, x => x.Light.Name == sessionFilter.LightName); // get all
			response.Results = response.Results;
			return response;
		}
	}
}
