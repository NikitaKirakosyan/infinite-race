using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class HUDController : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += Show;
            _gameManager.OnGameOver += Hide;
            
            Hide();
        }
        
        
        private void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}