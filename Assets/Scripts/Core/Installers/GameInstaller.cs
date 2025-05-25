using Core.Controller;
using Game;
using Game.EventBus;
using Game.Infra;
using Game.Services;
using Zenject;
using ResourceProvider = Game.Services.ResourceProvider;

namespace Core.Installers
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            BindServices();
            BindControllers();
        }

        private void BindServices()
        {
            Container.Bind<IControllerFactory>().To<ControllerFactory>().FromNew().AsSingle();
            Container.Bind<IBundleProvider>().To<ResourceProvider>().FromNew().AsTransient();
            Container.Bind<IEventBus>().To<EventBus>().FromNew().AsTransient();
        }

        private void BindControllers()
        {
            Container.Bind<MyRootController>().FromNew().AsSingle();
            Container.Bind<InitializeController>().FromNew().AsSingle();
            Container.Bind<GameController>().FromNew().AsSingle();
        }
    }
}