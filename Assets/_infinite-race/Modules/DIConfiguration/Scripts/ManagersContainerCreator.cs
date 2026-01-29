using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKLogger;
using Southbyte.ScreensSystem;
using UnityEngine;
using Zenject;

namespace Southbyte.DIConfiguration
{
    [EarlyInitialization]
    public class ManagersContainerCreator
    {
        public event Action OnServicesInitializationFailed;
        public event Action OnServicesInitializationCompleted;
        
        private EarlyScreenManager _earlyScreenManager;
        
#if !PROD
        private readonly Stopwatch _stopwatch = new ();
        private float _initializeTimeMarker;
        
        private float RealtimeSinceStartup => _initializeTimeMarker + InitializationProcessTime;
        private float InitializationProcessTime => (float)_stopwatch.Elapsed.TotalSeconds;
#endif
        
        [Inject]
        public async void Initialize(ManagersFacade.Factory factory, List<IAsyncInitializationService> asyncServices, EarlyScreenManager earlyScreenManager)
        {
            _earlyScreenManager = earlyScreenManager;
            
            try
            {
#if !PROD
                _initializeTimeMarker = Time.realtimeSinceStartup;
                _stopwatch.Start();
#endif
                using var poolObject = UnityEngine.Pool.ListPool<Task>.Get(out var tasks);
                
                foreach (var service in asyncServices)
                {
                    tasks.Add(service.StartInitializationAsync());
                }
                
                UpdateLoadingScreenProgress(tasks);
                
                await Task.WhenAll(tasks);
                
                factory.Create();
#if !PROD
                _stopwatch.Stop();
                DebugPro.Log($"Initialization completed, total take seconds:{Time.realtimeSinceStartup - _initializeTimeMarker}; realtimeSinceStartup:{RealtimeSinceStartup}");
#endif
                OnServicesInitializationCompleted?.Invoke();
            }
            catch (Exception e)
            {
                DebugPro.LogError($"Exception during initialization:\n{e}");
                OnServicesInitializationFailed?.Invoke();
            }
        }
        
        private async void UpdateLoadingScreenProgress(List<Task> tasksToCopy)
        {
            using var poolObject = UnityEngine.Pool.ListPool<Task>.Get(out var tasks);
            
            tasks.AddRange(tasksToCopy);
            
            var totalTasks = tasksToCopy.Count;
            var loadingScreen = _earlyScreenManager.Open<InitialScreen>(ScreenIds.InitialScreen);
            
            if (totalTasks == 0)
            {
                loadingScreen.SetProgress(1);
                await Task.Yield();
                loadingScreen.Close();
                
                return;
            }
            
            loadingScreen.SetProgress(0);
            
            while (tasks.Any())
            {
                await Task.WhenAny(tasks);
                
                for (var i = 0; i < tasks.Count; i++)
                {
                    var task = tasks[i];
                    if (task.IsCompleted)
                        tasks.RemoveAt(i--);
                }
                
                loadingScreen.SetProgress((totalTasks - tasks.Count) / (float)totalTasks);
            }
            
            loadingScreen.SetProgress(1);
            await Task.Yield();
            loadingScreen.Close();
        }
        
#if !PROD
        private void LogStatus(List<IAsyncInitializationService> services)
        {
            StringBuilder stringBuilder = new ();
            foreach (var service in services)
            {
                stringBuilder.Append(service.GetStatus()).Append("\n");
            }
            
            DebugPro.Log($"Initialization status: in process seconds:{InitializationProcessTime}, Realtime Since Startup:{RealtimeSinceStartup}\n{stringBuilder}");
        }
#endif
    }
}