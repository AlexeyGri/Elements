using UnityEngine;

namespace Game.Features.Room.Grid.Models
{
    public class Cell
    {
        private readonly Vector2 _endPosition;

        public Vector2 Position { get; private set; }
        
        public Cell(Vector2 position, float size)
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