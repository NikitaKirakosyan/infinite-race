using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NKLogger;
using Southbyte.CurrenciesSystem;
using Southbyte.DIConfiguration;
using Southbyte.YG2System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YG;
using Zenject;

namespace Southbyte.StoreSystem
{
    [EarlyInitialization]
    public class StoreManager : AsyncInitializationServiceBase
    {
        private const string StoreItemResourcesAddressablesPath = "StoreItemResources";
        
        public event Action<string> OnPurchaseSuccess;
        public event Action<string> OnPurchaseFailed;
        
        [Inject] private YG2Provider _yg2Provider;
        [Inject] private CurrenciesManager _currenciesManager;
        
        private StoreItemResources _storeItemResources;
        private AsyncOperationHandle<StoreItemResources> _handle;
        
        protected override List<Task> DependentServices => new ()
        {
            _yg2Provider.InitializationTask,
        };
        
        public List<string> PurchasedStoreItemIds
        {
            get => YG2.saves.storeSaveData.purchasedStoreItemIds;
            private set => YG2.saves.storeSaveData.purchasedStoreItemIds = value;
        }
        
        public bool WasPurchaseBonusUsed
        {
            get => YG2.saves.storeSaveData.wasPurchaseBonusUsed;
            set => YG2.saves.storeSaveData.wasPurchaseBonusUsed = value;
        }
        
        
        ~StoreManager()
        {
            YG2.onPurchaseSuccess -= OnPurchaseSuccessCallback;
            YG2.onPurchaseFailed -= OnPurchaseFailedCallback;
            
            if(_handle.IsValid())
                Addressables.Release(_handle);
        }
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            await InitAsync();
            await InitializationTask;
        }
        
        
        public bool TryGetStoreItemData(string purchaseId, out StoreItemData storeItemData)
        {
            return _storeItemResources.TryGetStoreItemData(purchaseId, out storeItemData);
        }
        
        public void Purchase(string purchaseId)
        {
            if(PurchasedStoreItemIds.Contains(purchaseId) &&
               TryGetStoreItemData(purchaseId, out var storeItemData) && 
               storeItemData.purchaseType == StoreItemPurchaseType.NonConsumable)
            {
                DebugPro.LogError($"Trying to purchase a non-consumable store item {purchaseId}!");
                return;
            }
            
            YG2.BuyPayments(purchaseId);
        }
        
        
        private async Task InitAsync()
        {
            _handle = Addressables.LoadAssetAsync<StoreItemResources>(StoreItemResourcesAddressablesPath);
            _handle.Completed += OnStoreItemResourcesLoaded;
            
            await _handle.Task;
            
            YG2.onPurchaseSuccess += OnPurchaseSuccessCallback;
            YG2.onPurchaseFailed += OnPurchaseFailedCallback;
            
            YG2.ConsumePurchases();
            TrySetInitializationResult(true);
        }
        
        private void OnPurchaseSuccessCallback(string purchaseId)
        {
            DebugPro.Log($"Purchase success! ID: {purchaseId}");
            
            var multiplier = !WasPurchaseBonusUsed && YG2.player.auth ? 2 : 1;
            
            _storeItemResources.TryGetStoreItemData(purchaseId, out var storeItemData);
            foreach(var currencyReward in storeItemData.currencyRewards)
            {
                _currenciesManager.ChangeCurrency(currencyReward.Key, currencyReward.Value * multiplier);
            }
            
            if(!WasPurchaseBonusUsed && YG2.player.auth)
                WasPurchaseBonusUsed = true;
            
            if(!PurchasedStoreItemIds.Contains(purchaseId))
                PurchasedStoreItemIds.Add(purchaseId);
            
            OnPurchaseSuccess?.Invoke(purchaseId);
            YG2.SaveProgress();
        }
        
        private void OnPurchaseFailedCallback(string purchaseId)
        {
            DebugPro.LogError($"Purchase failed! ID: {purchaseId}");
            OnPurchaseFailed?.Invoke(purchaseId);
        }
        
        private void OnStoreItemResourcesLoaded(AsyncOperationHandle<StoreItemResources> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                _storeItemResources = obj.Result;
                return;
            }
            
            DebugPro.LogError($"{nameof(StoreItemResources)} can't be loaded by path: {StoreItemResourcesAddressablesPath}");
        }
    }
}