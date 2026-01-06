using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using Southbyte.StoreSystem;
using UnityEngine;
using YG;
using Zenject;

namespace Southbyte.AdsSystem
{
    [EarlyInitialization]
    public class AdManager : AsyncInitializationServiceBase
    {
        public event Action OnInterstitialAdClosed;
        public event Action OnRewardedVideoAdClosed;
        
        [Inject] private StoreManager _storeManager;
        
        private bool _isInitialized;
        private InterstitialAdManager _interstitialAdManager;
        private RewardedVideoAdManager _rewardedVideoAdManager;
        private AdsConfig _adsConfig;
        private readonly Dictionary<string, (float lastViewedTime, int showAttempts)> _adItems = new ();
        
        protected override List<Task> DependentServices => new ()
        {
            _storeManager.InitializationTask,
        };
        
        public bool IsAdShowingAllowed { get; private set; } = true;
        
        
        ~AdManager()
        {
            _storeManager.OnPurchaseSuccess -= OnPurchaseSuccessCallback;
        }
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            Init();
            await InitializationTask;
        }
        
        
        public void Init()
        {
            DebugPro.Log("AdManager initialize requested.", this);
            
            if(_isInitialized)
            {
                DebugPro.LogError("AdManager is already initialized!", this);
                return;
            }
            
            _isInitialized = true;
            
            if(!CheckOnAdBlocker())
                _storeManager.OnPurchaseSuccess += OnPurchaseSuccessCallback;
            
            _adsConfig = Resources.Load<AdsConfig>("AdsSystem/AdsConfig");
            
            _interstitialAdManager = new InterstitialAdManager();
            _interstitialAdManager.Init();
            _interstitialAdManager.OnInterstitialAdClosed += HandleInterstitialAdClose;
            
            _rewardedVideoAdManager = new RewardedVideoAdManager();
            _rewardedVideoAdManager.Init();
            _rewardedVideoAdManager.OnRewardedVideoAdClosed += HandleRewardedVideoAdClosed;
            
            TrySetInitializationResult(true);
        }

        public void ShowInterstitialAd(string placementId = null)
        {
            if(!IsAdShowingAllowed)
                return;
            
            var isAbleToShow = true;
            
            if(!placementId.IsNullOrEmptyOrWhiteSpace())
            {
                if(!_adsConfig.TryGetItem(placementId, out var adItemData))
                {
                    DebugPro.LogError($"Unable to find ad placement id: {placementId}!");
                }
                else
                {
                    if(!_adItems.TryAdd(placementId, (Time.realtimeSinceStartup, 1)))
                    {
                        var item = _adItems[placementId];
                        
                        if(Time.realtimeSinceStartup - item.lastViewedTime < adItemData.secondsDelay || 
                           (adItemData.actionsDelay > 0 && item.showAttempts % adItemData.actionsDelay != 0))
                            isAbleToShow = false;
                        
                        if(isAbleToShow)
                            item.lastViewedTime = Time.realtimeSinceStartup;
                        
                        item.showAttempts++;
                        _adItems[placementId] = item;
                    }
                }
            }
            
            if(isAbleToShow)
                _interstitialAdManager.ShowInterstitialAd();
        }
        
        public void ShowRewardedVideoAd(Action onComplete = null)
        {
            _rewardedVideoAdManager.ShowInterstitialAd(onComplete);
        }
        
        
        private bool CheckOnAdBlocker()
        {
            if(_storeManager.PurchasedStoreItemIds.Contains(StoreItemIds.AdBlockerSku))
            {
                YG2.StickyAdActivity(false);
                IsAdShowingAllowed = false;
                return true;
            }
            
            YG2.StickyAdActivity(true);
            return false;
        }
        
        private void HandleInterstitialAdClose()
        {
            OnInterstitialAdClosed?.Invoke();
        }
        
        private void HandleRewardedVideoAdClosed()
        {
            OnRewardedVideoAdClosed?.Invoke();
        }
        
        private void OnPurchaseSuccessCallback(string purchaseId)
        {
            CheckOnAdBlocker();
        }
    }
}