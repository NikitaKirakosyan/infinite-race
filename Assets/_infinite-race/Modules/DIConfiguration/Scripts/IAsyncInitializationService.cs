using System.Threading.Tasks;

namespace Southbyte.DIConfiguration
{
    public interface IAsyncInitializationService
    {
        /// <summary>
        /// Returns a task this is completed if service is initialized and not completed if it hasn't initialized yet.
        /// Using this method to start initialization process is not recommended. Consider using a task from
        /// InitializationStatus class or task completion source.
        /// </summary>
        Task StartInitializationAsync();
#if !PROD
        string GetStatus();
#endif
    }
}