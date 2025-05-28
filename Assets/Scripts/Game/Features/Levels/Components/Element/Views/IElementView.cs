using Game.Features.Levels.Components.Element.Models;

namespace Game.Features.Levels.Components.Element.Views
{
    public interface IElementView
    {
        public int Id { get; }
        int Order { get; }
        
        void Setup(ElementModel model, int order);
        void Show();
        void Hide();
        void ShowDestroy();
        void MoveTo(Directions direction, float target, int order);
    }
}