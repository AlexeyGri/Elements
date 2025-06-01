using System;
using System.Collections.Generic;
using Game.Features.Levels.Components.Grid.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Grid.Views
{
    public class GridView : MonoBehaviour, IGridView
    {
        [SerializeField] private Transform _transform;

        private GridModel _model;
        private Vector2 _startPosition;
        private float _offset;

        public List<CellModel> Cells { get; private set; }
        public float CellsSize { get; private set; }

        public event Action PauseClicked = delegate { };

        public void Setup(GridModel model)
        {
            _model = model;
            
            CellsSize = (float) Screen.width / (model.Columns + 1);
            _offset = CellsSize / 100;

            var xPosition = _model.Columns * _offset / 2;
            _startPosition = new Vector2(_transform.position.x - xPosition, _transform.position.y);
            
            CalculateCells();
        }

        private void CalculateCells()
        {
            Cells = new List<CellModel>();

            for (var i = 0; i < _model.Rows; i++)
            {
                var rowPosition = _startPosition.y + i * _offset;
                for (var j = 0; j < _model.Columns; j++)
                {
                    var cellPosition = new Vector2(_startPosition.x + j * _offset, rowPosition);
                    var cell = new CellModel(cellPosition, _offset);
                    
                    Cells.Add(cell);
                }
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            PauseClicked.Invoke();
        }
    }
}