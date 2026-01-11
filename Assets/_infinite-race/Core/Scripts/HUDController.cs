using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private GameObject _hudRootPrefab;
        
        [Inject] private GameManager _gameManager;
        
        private DiContainer _diContainer;
        private GameObject _hudRoot;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += Show;
            _gameManager.OnGameOver += Hide;
            
            Hide();
        }
        
        
        public void Init(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _hudRoot = _diContainer.InstantiatePrefab(_hudRootPrefab, transform);
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