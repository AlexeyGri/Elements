using System.Collections.Generic;
using System.Threading;
using Core.Controller;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Levels.Events;
using Game.Features.Levels.Models;
using Game.Services;
using UnityEngine;

namespace Game.Features.Levels
{
    public class LevelsController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;
        private readonly IEventBus _eventBus;
        private readonly IControllerFactory _controllerFactory;
        private readonly List<LevelModel> _levels = new();
        
        private LevelController _levelController;
        private UniTaskCompletionSource _completionSource;
        private int _currentLevel;

        public LevelsController(IBundleProvider bundleProvider, IEventBus eventBus, IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
            _bundleProvider = bundleProvider;
            _eventBus = eventBus;
        }
        
        protected override async void OnStart()
        {
            await TryLoadLevelAsync(Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }
            
            _eventBus.Subscribe<LevelFinishedEvent>(OnLevelFinished);
            
            while (!Token.IsCancellationRequested)
            {
                CreateLevel();
                
                _eventBus.Invoke(new LevelLoadedEvent());

                await WaitLevelResult();
                
                RemoveController(_levelController);
            }
        }

        protected override void OnStop()
        {
            _eventBus.Unsubscribe<LevelFinishedEvent>(OnLevelFinished);
        }

        protected override void OnDispose()
        {
        }

        private void CreateLevel()
        {
            _levelController = _controllerFactory.CrateController<LevelController>();
            _levelController.SetArgs(_levels[_currentLevel]);
            AddController(_levelController);
        }

        private UniTask WaitLevelResult()
        {
            _completionSource = new UniTaskCompletionSource().WithToken(Token);
            return _completionSource.Task.SuppressCancellationThrow();
        }

        private async UniTask<bool> TryLoadLevelAsync(CancellationToken token)
        {
            var loadingResult = await UniTask.WhenAll(
                _bundleProvider.LoadAssetAsync<Object>($"{ResourcePaths.LevelPath}0", token),
                _bundleProvider.LoadAssetAsync<Object>($"{ResourcePaths.LevelPath}1", token),
                _bundleProvider.LoadAssetAsync<Object>($"{ResourcePaths.LevelPath}2", token))
                .SuppressCancellationThrow();

            if (loadingResult.IsCanceled)
            {
                return false;
            }
            
            _levels.Add(loadingResult.Result.Item1 as LevelModel);
            _levels.Add(loadingResult.Result.Item2 as LevelModel);
            _levels.Add(loadingResult.Result.Item3 as LevelModel);

            return true;
        }
        
        private void OnLevelFinished(LevelFinishedEvent e)
        {
            switch (e.Result)
            {
                case LevelResults.Finished:
                case LevelResults.Next:
                    if (++_currentLevel >= _levels.Count)
                    {
                        _currentLevel = 0;
                    }
                    break;
                case LevelResults.Restart:
                    break;
            }

            _completionSource.TrySetResult();
        }
    }
}