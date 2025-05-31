using UnityEngine;

namespace Game.Features.Levels.Components.Grid.Models
{
    public class CellModel
    {
        private readonly Vector2 _endPosition;

        public Vector2 Position { get; }
        
        public CellModel(Vector2 position, float offset)
        {
            Position = position;
            _endPosition = new Vector2(Position.x + offset, Position.y + offset);
        }

        public bool InPoint(Vector3 point)
        {
            var t = point.x < _endPosition.x
                   && point.x > Position.x
                   && point.y > Position.y
                   && point.y < _endPosition.y;

            return t;
        }
    }
}
