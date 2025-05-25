using System.Threading;
using Zenject;

namespace Core.Controller
{
    public class ControllerFactory : IControllerFactory
    {
        [Inject]
        private DiContainer _container;
        
        public T CrateRoot<T>(CancellationToken token) where T : RootController
        {
            var root = _container.Resolve<T>();
            root.SetCancellationToken(token);

            return root;
        }

        public T CrateController<T>() where T : ControllerBase
        {
            return _container.Resolve<T>();
        }
    }
}