using System;
using System.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using Southbyte.YG2System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YG;
using Zenject;
using Object = UnityEngine.Object;

namespace Southbyte.CurrenciesSystem
{
    [EarlyInitialization]
    public class CurrenciesManager : AsyncInitializationServiceBase
    {
        private const string CurrenciesConfigAddressablesPath = "CurrenciesConfig";
        
        public event Action<CurrencyType> OnCurrencyChanged;
        
        [Inject] private YG2Provider _yg2Provider;
        
        private AsyncOperationHandle<CurrenciesConfig> _handle;
        
        public CurrenciesConfig CurrenciesConfig { get; private set; }
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            _handle = Addressables.LoadAssetAsync<CurrenciesConfig>(CurrenciesConfigAddressablesPath);
            _handle.Completed += OnCurrenciesConfigLoaded;
            await _handle.Task;
            TrySetInitializationResult(true);
        }
        
        public void ChangeCurrency(CurrencyType currency, int delta)
        {
            if(delta == 0)
                return;
            
            var isSuccess = false;
            for(var i = 0; i < YG2.saves.currencies.Count; i++)
            {
                var savesCurrency = YG2.saves.currencies[i];
                if(savesCurrency.Key == currency)
                {
                    isSuccess = true;
                    savesCurrency.Value += delta;
                    YG2.saves.currencies[i] = savesCurrency;
                    break;
                }
            }
            
            if(!isSuccess)
                YG2.saves.currencies.Add(new (currency, delta));
            
            OnCurrencyChanged?.Invoke(currency);
        }
        
        public void SetCurrency(CurrencyType currency, int value)
        {
            var isSuccess = false;
            for(var i = 0; i < YG2.saves.currencies.Count; i++)
            {
                var savesCurrency = YG2.saves.currencies[i];
                if(savesCurrency.Key == currency)
                {
                    isSuccess = true;
                    savesCurrency.Value = value;
                    YG2.saves.currencies[i] = savesCurrency;
                    break;
                }
            }
            
            if(!isSuccess)
                YG2.saves.currencies.Add(new (currency, value));
            
            OnCurrencyChanged?.Invoke(currency);
        }
        
        public int GetCurrency(CurrencyType currency)
        {
            foreach(var currencyInfo in YG2.saves.currencies)
            {
                if(currencyInfo.Key == currency)
                    return currencyInfo.Value;
            }
            
            YG2.saves.currencies.Add(new (currency, 0));
            return 0;
        }
        
        public string GetTextSpriteTag(CurrencyType currency)
        {
            switch(currency)
            {
                case CurrencyType.Gold:
                    return "<sprite name=\"gold-currency-icon\">";
                
                default:
                    DebugPro.LogError($"Unexpected currency type {currency}!");
                    return "";
            }
        }
        
        public bool TryGetCurrencyIconSprite(CurrencyType currency, out Sprite sprite)
        {
            sprite = null;
            
            if(CurrenciesConfig.TryGetSettings(currency, out var settings))
            {
                sprite = settings.icon;
                return true;
            }
            
            return false;
        }
        
        public void AnimateAll(CurrencyType type)
        {
            var allActiveViews = Object.FindObjectsByType<CurrencyView>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach(var currencyView in allActiveViews)
            {
                if(currencyView.CurrencyType == type)
                    currencyView.Animate();
            }
        }
        
        
        private void OnCurrenciesConfigLoaded(AsyncOperationHandle<CurrenciesConfig> handle)
        {
            _handle.Completed -= OnCurrenciesConfigLoaded;
            
            if(handle.Status == AsyncOperationStatus.Failed)
            {
                DebugPro.LogError("Failed to load Currencies config!");
                return;
            }
            
            CurrenciesConfig =  handle.Result;
        }
    }
}