using Cysharp.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using YG;

namespace Southbyte.YG2System
{
    [EarlyInitialization]
    public class YG2Provider
    {
        public async UniTask Init()
        {
            DebugPro.Log("YG2.StartInit requested");
            YG2.StartInit();
            
            await UniTask.WaitWhile(() => !YG2.isSDKEnabled);
            DebugPro.Log($"YG2.isSDKEnabled: {YG2.isSDKEnabled}.");
            
            DebugPro.Log("GameReadyApi called.");
            YG2.GameReadyAPI();
        }
    }
}