using Game.Features.Room.Cell.Models;

namespace Game.Features.Room.Element.Views
{
    public interface IElementView
    {
        void Show();
        void Hide();
        void ShowDestroy();
        void MoveTo(Directions direction, float target);
    }
}