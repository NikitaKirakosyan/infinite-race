using ModestTree;
using Southbyte.AudioSystem;
using Southbyte.LevelGenerationSystem;
using Southbyte.RaceSystem;
using Southbyte.ScreensSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Southbyte.DIConfiguration
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private ManagersFacade _managersFacadePrefab;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private UILayers _uiLayers;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private DayNightManager _dayNightManager;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ShowcaseController _showcaseController;
        [SerializeField] private TrafficSpawner _trafficSpawner;


        public override void InstallBindings()
        {
            Container.BindFactory<ManagersFacade, ManagersFacade.Factory>().FromSubContainerResolve().ByNewContextPrefab(_managersFacadePrefab);
            
            Container.BindInstance(_eventSystem).AsSingle().NonLazy();
            Container.BindInstance(_uiLayers).AsSingle().NonLazy();
            Container.BindInstance(_audioManager).AsSingle().NonLazy();
            Container.BindInstance(_updateManager).AsSingle().Lazy();
            Container.BindInstance(_dayNightManager).AsSingle().Lazy();
            Container.BindInstance(_levelGenerator).AsSingle().Lazy();
            Container.BindInstance(_showcaseController).AsSingle().Lazy();
            Container.BindInstance(_trafficSpawner).AsSingle().Lazy();
            
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