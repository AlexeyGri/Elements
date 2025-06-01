using Core.Views;
using UnityEngine;

namespace Game.Features.Background.Views
{
    public class BackGroundView : ViewBase, IBackGroundView
    {
        [SerializeField] private Canvas _canvas;
        
        private void Awake()
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}