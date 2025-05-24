using Zenject;

namespace Core.Installers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameInstaller.Install(Container);
        }
    }
}