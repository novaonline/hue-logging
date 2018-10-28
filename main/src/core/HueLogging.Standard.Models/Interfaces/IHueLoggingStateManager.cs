using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	public interface IHueLoggingStateManager
	{
		/// <summary>
		/// The last event taken place.
		/// </summary>
		/// <returns></returns>
		Task<LightEvent> GetLastEvent();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="LightId"></param>
		/// <returns></returns>
		Task<LightEvent> GetLastEvent(string LightId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lightEvent"></param>
		/// <returns></returns>
		Task SaveAsLastLightEvent(LightEvent lightEvent);
	}
}
