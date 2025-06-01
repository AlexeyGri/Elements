using System;
using System.Threading;
using Core.Controller;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Infra.Events;
using UnityEngine;

namespace Game.Infra
{
    public class MyRootController : RootController
    {
        private readonly IControllerFactory _controllerFactory;
        private readonly IEventBus _eventBus;
        private readonly UniTaskCompletionSource _completion = new();

        private ControllerBase _initializeController;
        private ControllerBase _gameController;
        
        public MyRootController(IControllerFactory controllerFactory, IEventBus eventBus)
        {
            _controllerFactory = controllerFactory;
            _eventBus = eventBus;
        }
        
        protected override async void OnStart()
        {
            _completion.WithToken(Token);
            
            try
            {
                var initializeTask = await TryInitializeGameAsync(Token).SuppressCancellationThrow();
                if (initializeTask.IsCanceled || !initializeTask.Result)
                {
                    Stop();
                    Dispose();
                    
                    return;
                }

                _gameController = _controllerFactory.CrateController<GameController>();
                AddController(_gameController);

                await _completion.Task.SuppressCancellationThrow();
            }
            catch (Exception e)
            {
                Stop();
                Dispose();
                
                Debug.LogException(e);
            }
        }

        protected override void OnStop()
        {
            RemoveController(_initializeController);
        }

        protected override void OnDispose()
        {
        }

        private UniTask<bool> TryInitializeGameAsync(CancellationToken token)
        {
            var initializeCompletionSource = new UniTaskCompletionSource<bool>().WithToken(token);
            
            _eventBus.Subscribe<ResourcesPreloadedEvent>(OnResourcesPreloadedWithResult);
            
            _initializeController = _controllerFactory.CrateController<InitializeController>();
            AddController(_initializeController);

            return initializeCompletionSource.Task;

            void OnResourcesPreloadedWithResult(ResourcesPreloadedEvent e)
            {
                _eventBus.Unsubscribe<ResourcesPreloadedEvent>(OnResourcesPreloadedWithResult);
                
                RemoveController(_initializeController);
                
                initializeCompletionSource.TrySetResult(e.IsPreloaded);
            }
        }
    }
}