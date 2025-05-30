using Game.Features.Levels.Components.Element.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Views
{
    public interface IElementView
    {
        public int Id { get; }
        int Order { get; }
        
        void Initialize(ElementModel model);
        void Setup(Vector2 position, float size, int order);
        void Show();
        void Hide();
        void ShowDestroy();
        void MoveTo(Directions direction, float target, int order);
    }
}