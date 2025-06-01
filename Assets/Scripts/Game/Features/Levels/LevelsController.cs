using System.Collections.Generic;
using System.Threading;
using Core.Controller;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Levels.Events;
using Game.Features.Levels.Models;
using Game.Services;

namespace Game.Features.Levels
{
    public class LevelsController : ControllerBase
    {
        private const int LevelCount = 3;
        
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
            var result = await TryLoadLevelAsync(Token).SuppressCancellationThrow();
            if (result.IsCanceled || Token.IsCancellationRequested)
            {
                _eventBus.Invoke(new LevelsLoadingEvent(LevelsLoadingResult.Fail));
                
                return;
            }
            
            _eventBus.Invoke(new LevelsLoadingEvent(LevelsLoadingResult.Success));
            
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
            var levelLoadTasks = new List<UniTask<LevelModel>>();
            for (var i = 0; i < LevelCount; i++)
            {
                var loadTask = LoadLevelAsync($"{ResourcePaths.LevelPath}i", token);
                levelLoadTasks.Add(loadTask);
            }

            var loadingResult = await levelLoadTasks;
            if (token.IsCancellationRequested)
            {
                return false;
            }
            
            _levels.AddRange(loadingResult);
            
            return true;
        }

        private UniTask<LevelModel> LoadLevelAsync(string path, CancellationToken token)
        {
            return _bundleProvider.LoadAssetAsync<LevelModel>(path, token);
        }
        
        private void OnLevelFinished(LevelFinishedEvent e)
        {
            switch (e.Result)
            {
                case LevelResults.Finished:
                case LevelResults.Next:
                    if (++_currentLevel >= LevelCount)
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