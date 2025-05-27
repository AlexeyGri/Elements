using Core.Controller;
using Core.Utility;
using Game.Features.UI.Views;
using Game.Services;
using UnityEngine;

namespace Game.Features.UI
{
    public class InterfaceController : ControllerBase
    {
        private const string InterfacePath = "Prefabs/UI/UI";
        
        private readonly IBundleProvider _bundleProvider;

        private IInterfaceView _interfaceView;

        public InterfaceController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<InterfaceView>(InterfacePath, Token);
            _interfaceView = Object.Instantiate(prefab);
            
            ControllerResources.Add(new DisposableSource(() => Object.Destroy(_interfaceView.GameObject)));
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}