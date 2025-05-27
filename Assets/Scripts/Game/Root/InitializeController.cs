using System.Collections.Generic;
using Core.Controller;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Room.Element.Views;
using Game.Infra.Events;
using Game.Services;
using UnityEngine;

namespace Game.Infra
{
    public class InitializeController : ControllerBase
    {
        private const int ElementPreloadCount = 10;
        
        private IBundleProvider _bundleProvider;
        private IEventBus _eventBus;
        
        public InitializeController(IBundleProvider bundleProvider, IEventBus eventBus)
        {
            _bundleProvider = bundleProvider;
            _eventBus = eventBus;
        }
        
        protected override async void OnStart()
        {
            var elements = GetPreloadTasks("Prefabs/Elements/Fire/Fire");
            elements.AddRange(GetPreloadTasks("Prefabs/Elements/Water/Water"));

            await UniTask.WhenAll(elements);
            
            _eventBus.Invoke(new ResourcesPreloadedEvent(true));
        }
        
        private List<UniTask> GetPreloadTasks(string path)
        {
            var loads = new List<UniTask>(ElementPreloadCount);

            for (var i = 0; i < ElementPreloadCount; i++)
            {
                loads.Add(_bundleProvider.GetInstanceFromPoolAsync<GameObject>(ControllerResources, path, Token));
            }

            return loads;
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}