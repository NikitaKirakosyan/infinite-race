using System;
using NKLogger;
using YG;

namespace Southbyte.AdsSystem
{
    public class YandexInterstitialAdsProvider : IInterstitialAdsProvider
    {
        public event Action OnInterstitialAdClosed;
        
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
            YG2.onCloseInterAdv += OnCloseInterstitialAd;
            YG2.onErrorInterAdv += OnShowingError;
        }

        public void ShowInterstitialAd()
        {
            YG2.InterstitialAdvShow();
        }
        
        
        private void OnCloseInterstitialAd()
        {
            OnInterstitialAdClosed?.Invoke();
        }
        
        private void OnShowingError()
        {
            DebugPro.LogError("Something went wrong while trying to show interstitial ad!", this);
        }
    }
}