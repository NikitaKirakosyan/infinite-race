using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class FightTab : ScreenTabBase
    {
        [SerializeField] private Button _playButton;
        
        [Inject] private GameManager _gameManager;
        
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }
        
        
        private void OnPlayButtonClick()
        {
            _screenManager.Close(ScreenIds.MainScreen);
            _gameManager.Play();
        }
    }
}