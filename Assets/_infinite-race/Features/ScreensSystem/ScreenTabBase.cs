using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public abstract class ScreenTabBase<T> : MonoBehaviour where T : ScreenBase
    {
        [Inject] protected ScreenManager _screenManager;
        
        public T ScreenRoot { get; private set; }
        
        
        protected virtual void Awake()
        {
            if(_screenManager.TryGetScreen(out T screen))
                ScreenRoot = screen;
        }
        
        protected virtual void Start()
        {
            
        }
    }
}