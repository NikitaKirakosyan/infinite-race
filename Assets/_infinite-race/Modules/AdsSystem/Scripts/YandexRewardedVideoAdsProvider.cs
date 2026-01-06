using System;
using NKLogger;
using YG;

namespace Southbyte.AdsSystem
{
    public class YandexRewardedVideoAdsProvider : IRewardedVideoAdsProvider
    {
        public event Action OnRewardedVideoAdClosed;
        public bool IsInitialized { get; private set; }
        public bool IsReady => true;
        
        
        public void Init()
        {
            if(IsInitialized)
            {
                DebugPro.LogError("Interstitial ads provider is already initialized!", this);
                return;
            }
            
            IsInitialized = true;
            YG2.onCloseRewardedAdv += OnCloseRewardedAdv;
            YG2.onErrorInterAdv += OnShowingError;
        }
        
        public void ShowRewardedVideoAd(Action onCompleted)
        {
            if(onCompleted == null)
                YG2.RewardedAdvShow("default");
            else
                YG2.RewardedAdvShow("default", onCompleted);
        }
        
        
        private void OnCloseRewardedAdv()
        {
            OnRewardedVideoAdClosed?.Invoke();
        }
        
        private void OnShowingError()
        {
            DebugPro.LogError("Something went wrong while trying to show rewarded video ad!", this);
        }
    }
}