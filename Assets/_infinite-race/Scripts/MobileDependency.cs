using UnityEngine;

namespace Southbyte
{
    public class MobileDependency : MonoBehaviour
    {
        [SerializeField] private bool _activeOnMobile;
        
        
        private void Awake()
        {
            if(_activeOnMobile)
                gameObject.SetActive(Application.isMobilePlatform);
            else
                gameObject.SetActive(!Application.isMobilePlatform);
        }
    }
}