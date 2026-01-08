using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Southbyte.RaceSystem
{
    [EarlyInitialization]
    public class CarConfigsManager : AsyncInitializationServiceBase
    {
        private const string CarConfigsAddressablesPath = "CarConfig";
        
        private AsyncOperationHandle<IList<CarConfig>> _handle;
        
        public List<CarConfig> CarConfigs { get; private set; }
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            
            _handle = Addressables.LoadAssetsAsync<CarConfig>(CarConfigsAddressablesPath);
            await _handle.Task;
            
            if(_handle.Status == AsyncOperationStatus.Failed)
            {
                DebugPro.LogError("Unable to load car configs!");
                return;
            }
            
            CarConfigs = _handle.Result.ToList();
            TrySetInitializationResult(true);
        }
    }
}