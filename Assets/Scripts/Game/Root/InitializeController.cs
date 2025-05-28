using Core.Controller;
using Game.EventBus;
using Game.Infra.Events;
using Game.Services;
using UnityEngine;

namespace Game.Infra
{
    public class InitializeController : ControllerBase
    {
        private const int ElementPreloadCount = 20;

        private readonly IBundleProvider _bundleProvider;
        private readonly IEventBus _eventBus;

        public InitializeController(IBundleProvider bundleProvider, IEventBus eventBus)
        {
            _bundleProvider = bundleProvider;
            _eventBus = eventBus;
        }

        protected override async void OnStart()
        {
            var loadPrefabsOperation = await _bundleProvider.LoadAssetAsync<GameObject>(ResourcePaths.ElementPath, Token)
                .SuppressCancellationThrow();
            if (loadPrefabsOperation.IsCanceled)
            {
                _eventBus.Invoke(new ResourcesPreloadedEvent(false));
                return;
            }

            for (var i = 0; i < ElementPreloadCount; i++)
            {
                _bundleProvider.TryGetInstanceFromPool<GameObject>(ControllerResources, ResourcePaths.ElementPath);
            }

            _eventBus.Invoke(new ResourcesPreloadedEvent(true));
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}