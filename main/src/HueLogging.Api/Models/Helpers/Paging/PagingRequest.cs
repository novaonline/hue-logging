namespace HueLogging.Api.Models.Helpers.Paging
{
	public class PagingRequest
    {
		/// <summary>
		/// Not Zero based
		/// </summary>
		public int CurrentPage { get; set; }

		/// <summary>
		/// The number of items to return
		/// </summary>
		public int PageCount { get; set; }
		
	}
}
