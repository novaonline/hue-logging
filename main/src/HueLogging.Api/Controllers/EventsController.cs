using HueLogging.Api.Models.Helpers;
using HueLogging.Api.Models.Helpers.Paging;
using HueLogging.Api.Models.Response;
using HueLogging.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HueLogging.Api.Controllers
{
	public class EventsController : HueLoggingBaseController<EventsController>
	{
		protected readonly EventsService eventsService;

		public EventsController(EventsService eventsService, ILogger<EventsController> logger) : base(logger)
		{
			this.eventsService = eventsService;
		}

		[Produces(typeof(IEnumerable<EventResponse>))]
		[HttpGet]
		public IActionResult GetRecent(PagingRequest pagingRequest, TimeFrame timeFrame, string lightId)
		{
			try
			{
				if (!ModelState.IsValid) { return BadRequest(ModelState); }
				//var result = eventsService.GetRecentEvents(pagingRequest, timeFrame, lightId);
				return Ok(new EventResponse());
			}
			catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<EventResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}
		}

	}
}