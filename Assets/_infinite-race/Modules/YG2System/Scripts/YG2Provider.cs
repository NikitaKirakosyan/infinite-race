using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using YG;

namespace Southbyte.YG2System
{
    [EarlyInitialization]
    public class YG2Provider : AsyncInitializationServiceBase
    {
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            await Init();
            await InitializationTask;
        }
        
        
        private async UniTask Init()
        {
            DebugPro.Log("YG2.StartInit requested");
            YG2.StartInit();
            
            await UniTask.WaitWhile(() => !YG2.isSDKEnabled);
            DebugPro.Log($"YG2.isSDKEnabled: {YG2.isSDKEnabled}.");
            
            DebugPro.Log("GameReadyApi called.");
            YG2.GameReadyAPI();
            
            TrySetInitializationResult(true);
        }
    }
}