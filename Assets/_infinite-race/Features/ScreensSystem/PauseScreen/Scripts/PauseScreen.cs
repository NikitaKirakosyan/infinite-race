using Southbyte.LevelGenerationSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class PauseScreen : ScreenBase
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        
        [Inject] private GameManager _gameManager;
        [Inject] private ScreenManager _screenManager;
        [Inject] private LevelGenerator _levelGenerator;
        [Inject] private ShowcaseController _showcaseController;
        [Inject] private TrafficSpawner _trafficSpawner;
        
        public override string Id => ScreenIds.PauseScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            _continueButton.onClick.AddListener(OnContinueButtonClick);
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _menuButton.onClick.AddListener(OnMenuButtonClick);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _gameManager.Pause();
        }
        
        
        private void OnContinueButtonClick()
        {
            Close();
            _gameManager.Resume();
        }
        
        private void OnRestartButtonClick()
        {
            Close();
            _gameManager.Restart();
        }
        
        private void OnMenuButtonClick()
        {
            FindFirstObjectByType<HUDController>(FindObjectsInactive.Include).Hide();
            Destroy(FindFirstObjectByType<CarController>().gameObject);
            
            _showcaseController.Show();
            _trafficSpawner.CleanTraffic();
            _levelGenerator.SetLevelActive(false);
            Camera.main.gameObject.SetActive(false);
            
            _gameManager.Brake();
            Close();
            _screenManager.Open<MainScreen>(ScreenIds.MainScreen);
        }
    }
}