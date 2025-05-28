using Core.Controller;
using Core.Extensions;
using Game.Features.Levels.Components.Element.Views;
using Game.Features.Levels.Components.Grid.Views;
using Game.Features.Levels.Models;
using Game.Services;
using UnityEngine;

namespace Game.Features.Levels
{
    public class LevelController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;

        private LevelModel _model;
        private GridView _gridView;
        private IElementView[] _elementViews;

        public LevelController(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }
        
        public void SetArgs(LevelModel model)
        {
            _model = model;
        }

        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<GridView>(ResourcePaths.GridPath, Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }

            _gridView = this.Instantiate(ControllerResources, prefab);
            _gridView.Setup(_model.GridModel);

            GetViews();
        }

        private void GetViews()
        {
            _elementViews = new IElementView[_model.GridModel.Columns * _model.GridModel.Rows];

            for (var i = 0; i < _model.ElementModels.Count; i++)
            {
                var element = _model.ElementModels[i];
                if (element.Id < 0)
                {
                    continue;
                }
                
                var operationResult = _bundleProvider.TryGetInstanceFromPool<GameObject>(ControllerResources, ResourcePaths.ElementPath);
                 if (operationResult.Item1)
                 {
                     _elementViews[i] = operationResult.Item2.GetComponent<ElementView>();
                     _elementViews[i].Setup(element, i);
                     
                     _elementViews[i].Show();
                 }
            }
        }

        protected override void OnStop()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}