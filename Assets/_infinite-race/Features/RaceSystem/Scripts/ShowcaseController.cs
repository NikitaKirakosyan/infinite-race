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
        }
        
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}