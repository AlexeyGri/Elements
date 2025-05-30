using Core.Controller;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Levels.Events;
using Game.Features.Levels.Models;
using Game.Features.UI.Views;
using Game.Services;

namespace Game.Features.UI
{
    public class InterfaceController : ControllerBase
    {
        private const int Sec = 1000;
        
        private readonly IBundleProvider _bundleProvider;
        private readonly IEventBus _eventBus;

        private IInterfaceView _interfaceView;

        public InterfaceController(IBundleProvider bundleProvider, IEventBus eventBus)
        {
            _bundleProvider = bundleProvider;
            _eventBus = eventBus;
        }
        
        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<InterfaceView>(ResourcePaths.InterfacePath, Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }

            _interfaceView = this.Instantiate(ControllerResources, prefab);

            _interfaceView.RestartButtonClicked += OnRestartButtonClicked;
            _interfaceView.NextButtonClicked += OnNextButtonClicked;
            
            _interfaceView.Show();
        }
        protected override void OnStop()
        {
            _interfaceView.RestartButtonClicked -= OnRestartButtonClicked;
            _interfaceView.NextButtonClicked -= OnNextButtonClicked;
        }

        protected override void OnDispose()
        {
        }
        
        private void OnNextButtonClicked()
        {
            _eventBus.Invoke(new LevelFinishedEvent(LevelResults.Next));
            TemporarilyHide();
        }

        private void OnRestartButtonClicked()
        {
            _eventBus.Invoke(new LevelFinishedEvent(LevelResults.Restart));
            TemporarilyHide();
        }

        private async void TemporarilyHide()
        {
            _interfaceView.Hide();
            
            await UniTask.Delay(Sec);
            
            _interfaceView.Show();
        }
    }
}