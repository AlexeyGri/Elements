using Core.Controller;
using Core.Utility;
using Game.Features.Background.Views;
using Game.Services;
using UnityEngine;

namespace Game.Featuries.Background
{
    public class BackgroundController : ControllerBase
    {
        private const string BackgroundPath = "Prefabs/UI/BackgroundCanvas";
        
        private readonly IBundleProvider _bundleProvider;

        private IBackGroundView _backGroundView;
        
        public BackgroundController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<BackGroundView>(BackgroundPath, Token);
            _backGroundView = Object.Instantiate(prefab);
            
            ControllerResources.Add(new DisposableSource(() => Object.Destroy(_backGroundView.GameObject)));
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}