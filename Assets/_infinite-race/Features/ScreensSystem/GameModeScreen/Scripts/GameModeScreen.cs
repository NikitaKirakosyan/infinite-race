using Southbyte.ScreensSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte
{
    public class GameModeScreen : ScreenBase
    {
        [SerializeField] private Button _crushUntilButton;
        [SerializeField] private Button _timeAttackButton;
        [SerializeField] private Button _endlessButton;
        
        [Inject] private GameManager _gameManager;
        
        public override string Id => ScreenIds.GameModeScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            _crushUntilButton.onClick.AddListener(OnCrushUntilButtonClick);
            _timeAttackButton.onClick.AddListener(OnTimeAttackButtonClick);
            _endlessButton.onClick.AddListener(OnEndlessButtonClick);
        }
        
        
        private void OnCrushUntilButtonClick()
        {
            Close();
            _gameManager.SetMode(0);
            _gameManager.Play();
        }
        
        private void OnTimeAttackButtonClick()
        {
            Close();
            _gameManager.SetMode(1);
            _gameManager.Play();
        }
        
        private void OnEndlessButtonClick()
        {
            Close();
            _gameManager.SetMode(2);
            _gameManager.Play();
        }
    }
}