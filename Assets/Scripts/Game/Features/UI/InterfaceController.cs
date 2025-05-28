using Core.Controller;
using Core.Extensions;
using Core.Utility;
using Game.Features.UI.Views;
using Game.Services;
using UnityEngine;

namespace Game.Features.UI
{
    public class InterfaceController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;

        private IInterfaceView _interfaceView;

        public InterfaceController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<InterfaceView>(ResourcePaths.InterfacePath, Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }

            _interfaceView = this.Instantiate(ControllerResources, prefab);
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}