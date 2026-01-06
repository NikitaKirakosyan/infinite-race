using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Southbyte.DIConfiguration
{
    /// <summary>
    /// Important, if you inherited from this class, you should complete initialization task in child by calling TrySetInitializationResult or TrySetInitializationException
    /// </summary>
    public class AsyncInitializationServiceBase : IAsyncInitializationService
    {
        private TaskCompletionSource<bool> _initializationCompletionSource = new ();

        public bool IsInitializationCompleted => _initializationCompletionSource.Task.IsCompleted;
        public Task InitializationTask => _initializationCompletionSource.Task;
        
        protected virtual List<Task> DependentServices { get; }

#if !PROD
        private readonly Stopwatch _stopwatch = new ();
#endif
        

        public virtual async Task StartInitializationAsync()
        {
            if (!DependentServices.IsNullOrEmpty())
            {
                await Task.WhenAll(DependentServices);
            }
            
#if !PROD
            _stopwatch.Restart();
#endif
        }

#if !PROD
        public string GetStatus()
        {
            return $"{GetType().Name} initialization Task Status: {_initializationCompletionSource?.Task?.Status} duration:{_stopwatch.Elapsed}";
        }
#endif

        protected void TrySetInitializationResult(bool result)
        {
            if (_initializationCompletionSource != null && _initializationCompletionSource.Task.IsCompleted)
                return;
            
#if !PROD
            _stopwatch.Stop();
#endif
   
            _initializationCompletionSource?.TrySetResult(result);
        }
        
        protected void TrySetInitializationException(System.Exception exception)
        {
            if (_initializationCompletionSource != null && _initializationCompletionSource.Task.IsCompleted)
                return;
#if !PROD
            _stopwatch.Stop();
#endif

            _initializationCompletionSource?.TrySetException(exception);
        }
    }
}