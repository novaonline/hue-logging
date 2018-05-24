using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLogging.Api.Services;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HueLogging.Api.Controllers
{
    [Produces("application/json")]
	[Route("api/[controller]")]
	public class HueLoggingBaseController<T> : Controller
    {
		protected readonly ILogger<T> logger;

		public HueLoggingBaseController(ILogger<T> logger)
		{
			this.logger = logger;
		}

    }
}