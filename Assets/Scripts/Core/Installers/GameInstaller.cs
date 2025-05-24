using Core.Controller;
using Game;
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
        }

        private void BindControllers()
        {
            Container.Bind<RootController>().FromNew().AsSingle();
        }
    }
}