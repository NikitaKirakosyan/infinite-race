using Southbyte.LevelGenerationSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class EndScreen : ScreenBase
    {
        [SerializeField] private Button _adsButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _playButton;
        
        [Inject] private GameManager _gameManager;
        [Inject] private ScreenManager _screenManager;
        [Inject] private LevelGenerator _levelGenerator;
        [Inject] private ShowcaseController _showcaseController;
        [Inject] private TrafficSpawner _trafficSpawner;
        
        public override string Id => ScreenIds.EndScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            _adsButton.onClick.AddListener(OnAdsButtonClick);
            _menuButton.onClick.AddListener(OnMenuButtonClick);
            _playButton.onClick.AddListener(_gameManager.Play);
        }
        
        
        private void OnAdsButtonClick()
        {
            
        }
        
        private void OnMenuButtonClick()
        {
            _showcaseController.Show();
            _trafficSpawner.CleanTraffic();
            _levelGenerator.SetLevelActive(false);
            Camera.main.gameObject.SetActive(false);
            
            Close();
            _screenManager.Open<MainScreen>(ScreenIds.MainScreen);
        }
    }
}