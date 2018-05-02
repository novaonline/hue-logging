namespace HueLogging.Web.Models
{
    public class SimpleMessageViewModel
    {
		public string Message { get; set; }
		public string Title { get; set; }

		public SimpleMessageViewModel()
		{
			Title = "A Simple Message";
		}
	}
}
