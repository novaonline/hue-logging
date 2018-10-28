using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Models.Helpers.Paging
{
    public class PagingResponse
    {
		public int CurrentPage;
		public int PageCount;
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }
	}
}
