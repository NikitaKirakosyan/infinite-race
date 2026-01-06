using ModestTree;
using Zenject;

namespace Southbyte.DIConfiguration
{
    public class ManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var types = typeof(ManagersInstaller).Assembly.GetTypes();
            
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;
                
                var regularInitializationAttribute = type.TryGetAttribute<RegularInitializationAttribute>();
                if (regularInitializationAttribute != null)
                {
                    Container.BindInterfacesAndSelfTo(type).AsSingle().NonLazy();
                    if (regularInitializationAttribute.priority != 0)
                        Container.BindExecutionOrder(type, regularInitializationAttribute.priority);
                }
            }
        }
    }
}