using System;

namespace Southbyte.AdsSystem
{
    public interface IInterstitialAdsProvider
    {
        public event Action OnInterstitialAdClosed;
        
        public bool IsInitialized { get; }
        public bool IsReady { get; }
        
        
        public void Init();
        
        public void ShowInterstitialAd();
    }
}