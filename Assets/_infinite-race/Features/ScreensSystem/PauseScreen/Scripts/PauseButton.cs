using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class PauseButton : MonoBehaviour
    {
        [Inject] private ScreenManager _screenManager;
        
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }
        
        
        private void OnButtonClick()
        {
            _screenManager.Open<PauseScreen>(ScreenIds.PauseScreen);
        }
    }
}