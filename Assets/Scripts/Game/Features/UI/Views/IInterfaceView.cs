using System;
using UnityEngine;

namespace Game.Features.UI.Views
{
    public interface IInterfaceView
    {
        GameObject GameObject { get; }
        
        event Action RestartButtonClicked; 
        event Action NextButtonClicked; 
        
        void Show();
        void Hide();
    }
}