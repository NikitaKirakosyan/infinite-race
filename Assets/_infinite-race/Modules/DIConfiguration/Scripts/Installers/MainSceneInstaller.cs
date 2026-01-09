using ModestTree;
using Southbyte.AudioSystem;
using Southbyte.ScreensSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Southbyte.DIConfiguration
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private ManagersFacade _managersFacadePrefab;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private UILayers _uiLayers;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private DayNightManager _dayNightManager;


        public override void InstallBindings()
        {
            Container.BindFactory<ManagersFacade, ManagersFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab(_managersFacadePrefab);
            
            Container.BindInstance(_gameManager).AsSingle().NonLazy();
            Container.BindInstance(_eventSystem).AsSingle().NonLazy();
            Container.BindInstance(_uiLayers).AsSingle().NonLazy();
            Container.BindInstance(_audioManager).AsSingle().NonLazy();
            Container.BindInstance(_updateManager).AsSingle().Lazy();
            Container.BindInstance(_dayNightManager).AsSingle().NonLazy();
            
            var types = typeof(MainSceneInstaller).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;
                
                if (type.HasAttribute<EarlyInitializationAttribute>())
                    Container.BindInterfacesAndSelfTo(type).AsSingle().NonLazy();
            }
        }
    }
}