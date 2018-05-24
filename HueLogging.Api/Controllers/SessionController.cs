using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLogging.Api.Models.Helpers;
using HueLogging.Api.Models.Helpers.Paging;
using HueLogging.Api.Models.Response;
using HueLogging.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using HueLogging.Api.Models.Request.Filters;

namespace HueLogging.Api.Controllers
{
	public class SessionController : HueLoggingBaseController<SessionController>
	{

		protected readonly SessionService sessionService;

		public SessionController(SessionService sessionService, ILogger<SessionController> logger) : base(logger)
		{
			this.sessionService = sessionService;
		}

		[Produces(typeof(IEnumerable<SessionSummaryResponse>))]
		[HttpGet("GetRecent")]
		public IActionResult GetRecent(PagingRequest pagingRequest, TimeFrame timeFrame)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				var result = sessionService.GetRecentSessionsSummary(pagingRequest, timeFrame);
				return Ok(result);
			} catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<SessionSummaryResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}
		}

		[Produces(typeof(IEnumerable<SessionSummaryResponse>))]
		[HttpGet("GetByFilter")]
		public IActionResult GetByFilter(PagingRequest pagingRequest, SessionFilter sessionFilter)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				var result = sessionService.GetSessionsSummaryByFilter(pagingRequest, sessionFilter);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<SessionSummaryResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}
		}

		[Produces(typeof(IEnumerable<SessionResponse>))]
		[HttpGet("GetRecentInstances")]
		public IActionResult GetRecentInstances(PagingRequest pagingRequest, TimeFrame timeFrame)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				var result = sessionService.GetSessions(pagingRequest, timeFrame);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<SessionResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}
		}

		[Produces(typeof(IEnumerable<SessionResponse>))]
		[HttpGet("GetRecentInstancesByFilter")]
		public IActionResult GetRecentInstancesByFilter(PagingRequest pagingRequest, SessionFilter sessionFilter)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				var result = sessionService.GetSessionsByFilter(pagingRequest, sessionFilter);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<SessionResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}
		}
	}
}