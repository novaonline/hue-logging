using HueLogging.Api.Models.Helpers.Paging;
using System.Collections.Generic;

namespace HueLogging.Api.Models.Response
{
	public class BaseResponse<T>
    {
		public IEnumerable<T> Results { get; set; }

		public PagingResponse Page { get; set; }
	}
}
