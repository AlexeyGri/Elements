using System;

namespace Game.Features.UI.Views
{
    public interface IInterfaceView
    {
        event Action RestartButtonClicked; 
        event Action NextButtonClicked; 
        
        void Show();
        void Hide();
    }
}