namespace Core.Controller
{
    public interface IControllerFactory
    {
        T CrateController<T>() where T : ControllerBase;
    }
}