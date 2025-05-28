using UnityEngine;

namespace Game.Features.Levels.Components.Grid.Models
{
    public class CellModel
    {
        private readonly Vector2 _endPosition;

        public Vector2 Position { get; private set; }
        
        public CellModel(Vector2 position, float size)
        {
            Position = position;
            _endPosition = new Vector2(Position.x + size, Position.y + size);
        }

        public bool InPoint(Vector3 point)
        {
            return point.x > Position.x
                   && point.x < _endPosition.x
                   && point.y > Position.y
                   && point.y < _endPosition.y;
        }
    }
}
