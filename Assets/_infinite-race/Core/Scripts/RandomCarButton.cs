using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Southbyte
{
    public class RandomCarButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private CarSpawner _carSpawner;
        
        
        private void Reset()
        {
            _button = GetComponent<Button>();
        }
        
        private void Awake()
        {
            _button.onClick.AddListener(() => _carSpawner.Spawn());
        }
    }
}