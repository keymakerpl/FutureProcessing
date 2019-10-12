using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace PersonManager
{
    public class PersonManagerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IConfig, Config>();
            containerRegistry.Register<IPersonManager, PersonManager>();
        }
    }
}