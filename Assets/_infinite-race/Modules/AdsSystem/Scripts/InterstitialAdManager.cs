using System;
using NKLogger;

namespace Southbyte.AdsSystem
{
    public class InterstitialAdManager
    {
        public Action OnInterstitialAdClosed;
        
        private bool _isInitialized;
        private IInterstitialAdsProvider _interstitialAdsProvider;


        public void Init()
        {
            DebugPro.Log("InterstitialAdManager initialize requested.", this);

            if(_isInitialized)
            {
                DebugPro.LogError("InterstitialAdManager is already initialized!", this);
                return;
            }

            _isInitialized = true;

            _interstitialAdsProvider = GetInterstitialAdsProvider();
            _interstitialAdsProvider.Init();
            _interstitialAdsProvider.OnInterstitialAdClosed += HandleInterstitialAdClose;
        }

        public void ShowInterstitialAd()
        {
            DebugPro.Log($"Interstitial ad display requested. Provider: {_interstitialAdsProvider.GetType()}", this);

            if(!_interstitialAdsProvider.IsReady)
            {
                DebugPro.Log("Interstitial ad is not ready yet.", this);
                return;
            }

            _interstitialAdsProvider.ShowInterstitialAd();
        }


        private IInterstitialAdsProvider GetInterstitialAdsProvider()
        {
            IInterstitialAdsProvider interstitialAdsProvider;

#if InterstitialAdv_yg
            interstitialAdsProvider = new YandexInterstitialAdsProvider();
#else
            throw new PlatformNotSupportedException("This platform does not support interstitial ads.");
#endif
            return interstitialAdsProvider;
        }
        
        private void HandleInterstitialAdClose()
        {
            OnInterstitialAdClosed?.Invoke();
        }
    }
}