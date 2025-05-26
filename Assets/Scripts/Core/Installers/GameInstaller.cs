using Core.Controller;
using Game.EventBus;
using Game.Features.Levels;
using Game.Features.UI;
using Game.Featuries.Background;
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
            Container.Bind<IEventBus>().To<EventBus>().FromNew().AsTransient().NonLazy();
            
            Container.Bind<IControllerFactory>().To<ControllerFactory>().FromNew().AsSingle();
            Container.Bind<IBundleProvider>().To<ResourceProvider>().FromNew().AsCached();
            
            Container.Bind<TouchScreenInput>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<MyRootController>().FromNew().AsSingle();
            Container.Bind<InitializeController>().FromNew().AsSingle();
            Container.Bind<GameController>().FromNew().AsSingle();
            Container.Bind<BackgroundController>().FromNew().AsSingle();
            Container.Bind<InterfaceController>().FromNew().AsSingle();
            Container.Bind<LevelsController>().FromNew().AsSingle();
        }
    }
}