using Core.Controller;
using Core.Extensions;
using Game.Features.Background.Views;
using Game.Services;

namespace Game.Featuries.Background
{
    public class BackgroundController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;

        private IBackGroundView _backGroundView;
        
        public BackgroundController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<BackGroundView>(ResourcePaths.BackgroundPath, Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }
            
            _backGroundView = this.Instantiate(ControllerResources, prefab);
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}