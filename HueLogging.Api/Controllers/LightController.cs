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
	public class LightController : HueLoggingBaseController<LightController>
	{
		protected readonly LightService lightService;

		public LightController(LightService lightService, ILogger<LightController> logger) : base(logger)
		{
			this.lightService = lightService;
		}

		[Produces(typeof(IEnumerable<LightResponse>))]
		[HttpGet]
		public IActionResult Get(PagingRequest pagingRequest)
		{
			try
			{
				if(!ModelState.IsValid) { return BadRequest(ModelState); }
				var result = lightService.GetLightResult(pagingRequest);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				logger.LogWarning(ex, ex.Message);
				ModelState.AddModelError<LightResponse>(x => x.Page, ex.Message);
				return BadRequest(ModelState);
			}

		}
	}
}