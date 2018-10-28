
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
    public interface IHueLoggingManager
    {
		/// <summary>
		/// Begin
		/// </summary>
		Task Start(bool shouldStartNewSession);

		/// <summary>
		/// If precondition has failed log event should not continue.
		/// </summary>
		/// <returns></returns>
		Task<bool> HasPassedPreCondition();

		/// <summary>
		/// Log the event
		/// </summary>
		Task LogEvent(bool shouldStartNewSession);
	}
}
