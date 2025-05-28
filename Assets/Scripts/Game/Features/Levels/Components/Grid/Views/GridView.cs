using System.Collections.Generic;
using Core.Views;
using Game.Features.Levels.Components.Grid.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Grid.Views
{
    public class GridView : ViewBase, IGridView
    {
        [SerializeField] private Transform _transform;

        private GridModel _model;
        private float _cellSize;
        private Vector2 _startPosition;

        public List<CellModel> Cells { get; private set; }
        
        public void Setup(GridModel model)
        {
            _model = model;
            
            _cellSize = (float) Screen.width / (model.Columns + 1);
            _startPosition = new Vector2(_cellSize / 2, _transform.position.y);
            
            CalculateCells();
        }

        private void CalculateCells()
        {
            Cells = new List<CellModel>();

            for (var i = 0; i < _model.Rows; i++)
            {
                var rowPosition = _startPosition.y + i * _cellSize;
                for (var j = 0; j < _model.Columns; j++)
                {
                    var cellPosition = new Vector2(_startPosition.x + j * _cellSize, rowPosition);
                    var cell = new CellModel(cellPosition, _cellSize);
                    
                    Cells.Add(cell);
                }
            }
        }
    }
}