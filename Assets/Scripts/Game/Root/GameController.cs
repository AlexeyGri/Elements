using Core.Controller;
using Cysharp.Threading.Tasks;
using Game.Features.Levels;
using Game.Features.UI;
using Game.Featuries.Background;

namespace Game.Infra
{
    public class GameController : ControllerBase
    {
        private readonly IControllerFactory _controllerFactory;
        public GameController(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }
        
        protected override async void OnStart()
        {
            await UniTask.WhenAll(
                RunController<BackgroundController>(),
                RunController<InterfaceController>(),
                RunController<LevelsController>());
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }

        private UniTask RunController<T>() where T : ControllerBase
        {
            var child = _controllerFactory.CrateController<T>();
            AddController(child);
            
            return UniTask.CompletedTask;
        }
    }
}