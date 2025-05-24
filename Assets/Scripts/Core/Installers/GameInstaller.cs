using Core.Controller;
using Game;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IControllerFactory>().To<ControllerFactory>().FromNew().AsSingle();
            Container.Bind<RootController>().FromNew().AsSingle();
        }
    }
}