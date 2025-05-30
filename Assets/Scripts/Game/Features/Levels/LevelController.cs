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
            CreateGrid();
        }

        protected override void OnStop()
        {
            foreach (var elementView in _elementViews)
            {
                elementView.Hide();
            }
        }

        protected override void OnDispose()
        {
        }
        
        private void GetViews()
        {
            _elementViews = new IElementView[_model.GridModel.Columns * _model.GridModel.Rows];

            for (var i = 0; i < _model.ElementModels.Count; i++)
            {
                var operationResult =
                    _bundleProvider.TryGetInstanceFromPool<ElementView>(ControllerResources, ResourcePaths.ElementPath);
                if (operationResult.Item1)
                {
                    _elementViews[i] = operationResult.Item2;
                    _elementViews[i].Initialize(_model.ElementModels[i]);
                }
            }
        }

        private void CreateGrid()
        {
            var cells = _gridView.Cells;
            for (var i = 0; i < cells.Count; i++)
            {
                _elementViews[i].Setup(cells[i].Position, _gridView.CellsSize, i);
                _elementViews[i].Show();
            }
        }
    }
}
