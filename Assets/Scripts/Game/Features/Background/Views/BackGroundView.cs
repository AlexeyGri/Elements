using UnityEngine;

namespace Game.Features.Background.Views
{
    public class BackGroundView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        private void Awake()
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}