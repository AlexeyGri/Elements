using Core.Controller;
using Core.Extensions;
using Game.Features.Background.Views;
using Game.Services;
using UnityEngine;

namespace Game.Featuries.Background
{
    public class BackgroundController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;

        private BackGroundView _backGround;

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
            
            this.Instantiate(ControllerResources, prefab);
            _backGround = Object.Instantiate(prefab);
        }

        protected override void OnStop()
        {
            Object.Destroy(_backGround);
        }

        protected override void OnDispose()
        {
        }
    }
}