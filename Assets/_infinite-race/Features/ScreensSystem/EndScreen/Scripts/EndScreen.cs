using Southbyte.AdsSystem;
using Southbyte.CurrenciesSystem;
using Southbyte.LevelGenerationSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class EndScreen : ScreenBase
    {
        [SerializeField] private RaceResultView _raceResultView;
        [SerializeField] private Button _adsButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _playButton;
        
        [Inject] private GameManager _gameManager;
        [Inject] private ScreenManager _screenManager;
        [Inject] private LevelGenerator _levelGenerator;
        [Inject] private ShowcaseController _showcaseController;
        [Inject] private TrafficSpawner _trafficSpawner;
        [Inject] private AdsManager _adsManager;
        [Inject] private CurrenciesManager _currenciesManager;
        
        private int _money;
        private bool _rewardReceived;
        
        public override string Id => ScreenIds.EndScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            _adsButton.onClick.AddListener(OnAdsButtonClick);
            _menuButton.onClick.AddListener(OnMenuButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }
        
        
        public override void Close()
        {
            base.Close();
            YG2.SaveProgress();
        }
        
        public void Setup(float score, float distance, int money, float bestScore, float bestDistance)
        {
            _adsButton.gameObject.SetActive(true);
            _money = money;
            _raceResultView.Setup(score, distance, _money, bestScore, bestDistance);
        }
        
        
        private void RewardIfNeed()
        {
            if(!_rewardReceived && _money > 0)
            {
                _currenciesManager.ChangeCurrency(CurrencyType.Money, _money);
            }
            
            _rewardReceived = false;
        }
        
        private void OnAdsButtonClick()
        {
            _adsButton.gameObject.SetActive(false);
            _adsManager.ShowRewardedVideoAd(() =>
            {
                _rewardReceived = true;
                var reward = _money * 2;
                _currenciesManager.ChangeCurrency(CurrencyType.Money, reward);
            });
        }
        
        private void OnMenuButtonClick()
        {
            _showcaseController.Show();
            _trafficSpawner.CleanTraffic();
            _levelGenerator.SetLevelActive(false);
            Camera.main.gameObject.SetActive(false);
            
            RewardIfNeed();
            _screenManager.Close(ScreenIds.EndScreen);
            _screenManager.Open<MainScreen>(ScreenIds.MainScreen);
        }
        
        private void OnPlayButtonClick()
        {
            RewardIfNeed();
            _screenManager.Close(ScreenIds.EndScreen);
            _gameManager.Restart();
        }
    }
}