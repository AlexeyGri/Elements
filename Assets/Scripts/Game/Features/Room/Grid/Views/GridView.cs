using System.Collections.Generic;
using UnityEngine;

namespace Game.Features.Room.Grid.Views
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private int _columns;
        [SerializeField] private int _rows;
        [SerializeField] private Transform _transform;

        private float _cellSize;
        private Vector2 _startPosition;

        private void Start()
        {
            Setup();
        }

        public List<Models.Cell> Cells { get; private set; }
        
        public void Setup()
        {
            _cellSize = (float) Screen.width / (_columns + 1);

            CalculateOffset();
            CalculateCells();
        }

        private void CalculateOffset()
        {
            var offset = Screen.width - _cellSize * _columns;
            _startPosition = new Vector2(offset / 2, _transform.position.y);
        }

        private void CalculateCells()
        {
            Cells = new List<Models.Cell>();

            for (var i = 0; i < _rows; i++)
            {
                var rowPosition = _startPosition.y + i * _cellSize;
                for (var j = 0; j < _columns; j++)
                {
                    var cellPosition = new Vector2(_startPosition.x + j * _cellSize, rowPosition);
                    var cell = new Models.Cell(cellPosition, _cellSize);
                    
                    Cells.Add(cell);
                }
            }
        }
    }
}