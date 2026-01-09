using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class ShowcaseController : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += Hide;
            _gameManager.OnGameOver += Show;
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