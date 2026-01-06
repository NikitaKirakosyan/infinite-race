using System;
using NKLogger;

namespace Southbyte.AdsSystem
{
    public class RewardedVideoAdManager
    {
        public Action OnRewardedVideoAdClosed;
        
        private bool _isInitialized;
        private IRewardedVideoAdsProvider _rewardedVideoAdsProvider;
        
        public void Init()
        {
            DebugPro.Log("InterstitialAdManager initialize requested.", this);
            
            if(_isInitialized)
            {
                DebugPro.LogError("InterstitialAdManager is already initialized!", this);
                return;
            }
            
            _isInitialized = true;
            
            _rewardedVideoAdsProvider = GetRewardedVideoAdsProvider();
            _rewardedVideoAdsProvider.Init();
            _rewardedVideoAdsProvider.OnRewardedVideoAdClosed += HandleRewardedVideoAdClose;
        }
        
        public void ShowInterstitialAd(Action onComplete = null)
        {
            DebugPro.Log($"Interstitial ad display requested. Provider: {_rewardedVideoAdsProvider.GetType()}", this);
            
            if(!_rewardedVideoAdsProvider.IsReady)
            {
                DebugPro.Log("Interstitial ad is not ready yet.", this);
                return;
            }
            
            _rewardedVideoAdsProvider.ShowRewardedVideoAd(onComplete);
        }
        
        
        private IRewardedVideoAdsProvider GetRewardedVideoAdsProvider()
        {
            IRewardedVideoAdsProvider rewardedVideoAdsProvider;
            
#if InterstitialAdv_yg
            rewardedVideoAdsProvider = new YandexRewardedVideoAdsProvider();
#else
            throw new PlatformNotSupportedException("This platform does not support interstitial ads.");
#endif
            return rewardedVideoAdsProvider;
        }
        
        private void HandleRewardedVideoAdClose()
        {
            OnRewardedVideoAdClosed?.Invoke();
        }
    }
}