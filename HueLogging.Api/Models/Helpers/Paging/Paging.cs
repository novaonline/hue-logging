using System;
using System.Collections.Generic;
using System.Linq;

namespace HueLogging.Api.Models.Helpers.Paging
{
	public class Paging
	{
		public PagingRequest PagingRequest { get; }
		public IEnumerable<int> AllowedRange { get; }
		public int TotalCount { get; }
		public PagingResponse PagingResponse;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="totalSize">Count (i.e. Not zero based)</param>
		/// <exception cref="InvalidOperationException">When the request is not in range</exception>
		public Paging(PagingRequest request, int totalSize)
		{
			this.TotalCount = totalSize;
			this.PagingRequest = request;
			try
			{
				this.AllowedRange = Enumerable.Range(1, (totalSize / request.PageCount));
			}
			catch (DivideByZeroException)
			{
				this.AllowedRange = Enumerable.Empty<int>();
			}
			if(!AllowedRange.Contains(request.CurrentPage))
			{
				throw new InvalidOperationException($"The current page ({request.CurrentPage}) does not exist in range");
			}

			PagingResponse = new PagingResponse
			{
				TotalPages = this.AllowedRange.Max(),
				CurrentPage = PagingRequest.CurrentPage,
				PageCount = PagingRequest.PageCount,
				TotalCount = this.TotalCount
			};
		}
	}

	public static class PagingExtension
	{
		public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, Paging page)
		{
			var request = page.PagingRequest;
			return source.Skip((request.CurrentPage - 1) * request.PageCount).Take(request.PageCount);
		}
	}
}
