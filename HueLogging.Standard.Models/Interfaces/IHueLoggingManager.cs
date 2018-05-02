
namespace HueLogging.Standard.Models.Interfaces
{
    public interface IHueLoggingManager
    {
		/// <summary>
		/// Begin
		/// </summary>
		void Start(bool shouldStartNewSession);

		/// <summary>
		/// If precondition has failed log event should not continue.
		/// </summary>
		/// <returns></returns>
		bool HasPassedPreCondition();

		/// <summary>
		/// Log the event
		/// </summary>
		void LogEvent(bool shouldStartNewSession);
	}
}
