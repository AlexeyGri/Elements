using UnityEngine;

namespace Game.Features.UI.Views
{
    public interface IInterfaceView
    {
        GameObject GameObject { get; }
        
        void Show();
        void Hide();
    }
}