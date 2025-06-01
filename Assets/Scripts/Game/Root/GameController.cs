using Core.Controller;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Levels;
using Game.Features.Levels.Events;
using Game.Features.Levels.Models;
using Game.Features.UI;
using Game.Featuries.Background;
using UnityEngine.Device;

namespace Game.Infra
{
    public class GameController : ControllerBase
    {
        private readonly IControllerFactory _controllerFactory;
        private readonly IEventBus _eventBus;
        public GameController(IControllerFactory controllerFactory, IEventBus eventBus)
        {
            _controllerFactory = controllerFactory;
            _eventBus = eventBus;
        }
        
        protected override async void OnStart()
        {
            _eventBus.Subscribe<LevelsLoadingEvent>(OnLevelsLoadingEvent);
            
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
        
        private static void OnLevelsLoadingEvent(LevelsLoadingEvent e)
        {
            if (e.Result is LevelsLoadingResult.Fail)
            {
                Application.Quit();
            }
        }
    }
}