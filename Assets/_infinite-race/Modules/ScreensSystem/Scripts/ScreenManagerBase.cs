using System;
using System.Collections.Generic;
using System.Linq;
using NKLogger;
using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public abstract class ScreenManagerBase
    {
        public event Action<ScreenBase> OnScreensStackChanged;

        [Inject] private IInstantiator _instantiator;
        [Inject] private UILayers _uiLayers;

        private readonly Stack<ScreenBase> _screensStack = new (2);
        private readonly Dictionary<string, ScreenBase> _cachedScreens = new ();
        
        
        public void InjectContainer(DiContainer container)
        {
            _instantiator = container;
        }
        
        public ScreenBase GetTopScreen()
        {
            return _screensStack.IsNullOrEmpty() ? null : _screensStack.Peek();
        }

        public bool HasAnyActiveScreen()
        {
            return GetTopScreen() != null;
        }
        
        public bool TryGetScreen<T>(string screenId, out T screen) where T : ScreenBase
        {
            screen = null;
            
            foreach(var keyValuePair in _cachedScreens)
            {
                if(keyValuePair.Key == screenId)
                {
                    screen = keyValuePair.Value as T;
                    return true;
                }
            }
            
            return false;
        }
        
        public bool TryGetScreen<T>(out T screen) where T : ScreenBase
        {
            screen = null;
            
            foreach(var keyValuePair in _cachedScreens)
            {
                if(keyValuePair.Value is T targetScreen)
                {
                    screen = targetScreen;
                    return true;
                }
            }
            
            return false;
        }
        
        public T Open<T>(string screenId, bool isImmediately = false) where T : ScreenBase
        {
            if(screenId.IsNullOrEmptyOrWhiteSpace())
            {
                DebugPro.LogError($"Can't open screen! Parameter {nameof(screenId)} is null or empty.", this);
                return null;
            }

            if(!_screensStack.IsNullOrEmpty() && _screensStack.Peek().Id == screenId)
            {
                DebugPro.LogError($"Trying open already opened screen: {screenId}!", this);
                return null;
            }
            
            if(_cachedScreens.TryGetValue(screenId, out var screen))
            {
                screen.Open<T>();
                return (T) screen;
            }
            
            var screenPath = ScreensRegistry.GetScreenPath(screenId); 
            screen = CreateScreen<T>(screenPath);

            if(screen == null)
            {
                DebugPro.LogError($"Screen {screenId} creating is unable! Can't find screen reference in: [Resources/Prefabs/Screens/].", this);
                return default;
            }
            
            _cachedScreens.Add(screenId, screen);

            screen.OnOpened += () => OnScreenOpenedHandle(screen);
            screen.OnClosed += OnScreenClosedHandle;
            
            if(isImmediately) 
                screen.OpenImmediately<T>();
            else 
                screen.Open<T>();
            
            return (T) screen;
        }

        public ScreenBase Open(string screenId, bool isImmediately = false)
        {
            return Open<ScreenBase>(screenId, isImmediately);
        }
        
        public void Close(string screenId)
        {
            if(string.IsNullOrEmpty(screenId) || string.IsNullOrWhiteSpace(screenId))
            {
                DebugPro.LogError($"Can't close screen! Parameter {nameof(screenId)} is null or empty!", this);
                return;
            }
            
            if(!_cachedScreens.TryGetValue(screenId, out var screen))
            {
                DebugPro.LogError($"Can't close screen! The screen with id: [{nameof(screenId)}] doesn't exists!", this);
                return;
            }
            
            screen.Close();
        }
        
        public void CloseAll()
        {
            if(_screensStack.IsNullOrEmpty())
                return;
            
            for(var i = 0; i < _screensStack.Count; i++)
            {
                var screen = _screensStack.Peek();
                screen.Close();
            }
        }
        
        
        private T CreateScreen<T>(string path) where T : ScreenBase
        {
            var prefab = Resources.Load<T>(path);
            var screen = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            screen.transform.SetParent(screen.ParentLayer, false);
            return screen == null ? null : screen;
        }

        private void OnScreenOpenedHandle(ScreenBase openedScreen)
        {
            if(!_screensStack.IsNullOrEmpty() && _screensStack.TryPeek(out var screenToSleep) && openedScreen is not PopupBase)
                screenToSleep.Sleep();
            
            _screensStack.Push(openedScreen);
            OnScreensStackChanged?.Invoke(openedScreen);
        }
        
        private void OnScreenClosedHandle()
        {
            var closedScreen = _screensStack.Pop();
            OnScreensStackChanged?.Invoke(closedScreen);

            if(_screensStack.IsNullOrEmpty() || !_screensStack.TryPeek(out var screenToWakeUp) || closedScreen is PopupBase)
                return;
            
            screenToWakeUp.WakeUp();
        }
    }
}