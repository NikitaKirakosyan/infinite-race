using System;

namespace Southbyte.AdsSystem
{
    public interface IRewardedVideoAdsProvider
    {
        public event Action OnRewardedVideoAdClosed;
        
        public bool IsInitialized { get; }
        public bool IsReady { get; }
        
        
        public void Init();
        
        public void ShowRewardedVideoAd(Action onCompleted);
    }
}