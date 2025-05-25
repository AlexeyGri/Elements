using System.Threading;

namespace Core.Controller
{
    public interface IControllerFactory
    {
        T CrateRoot<T>(CancellationToken token) where T : RootController;
        T CrateController<T>() where T : ControllerBase;
    }
}