using Zenject;

namespace Core.Controller
{
    public class ControllerFactory : IControllerFactory
    {
        [Inject]
        private DiContainer _container;

        public T CrateController<T>() where T : ControllerBase
        {
            return _container.Resolve<T>();
        }
    }
}