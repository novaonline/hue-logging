using HueLogging.Api.Models;
using HueLogging.Api.Models.Helpers.Paging;
using HueLogging.Api.Models.Response;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Services
{
	public class LightService
	{
		private readonly IHueLoggingRepo hueLoggingRepo;

		public LightService(IHueLoggingRepo hueLoggingRepo)
		{
			this.hueLoggingRepo = hueLoggingRepo;
		}

		public LightResponse GetLightResult(PagingRequest pagingRequest)
		{
			var lights = hueLoggingRepo.GetLights();
			var paging = new Paging(pagingRequest, lights.AsQueryable().Count());
			var response = new LightResponse
			{
				Results = lights.GetPage(paging),
				Page = paging.PagingResponse
			};
			return response;

		}
	}
}
