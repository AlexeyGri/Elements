using UnityEngine;

namespace Game.Features.Background.Views
{
    public class BackGroundView : MonoBehaviour, IBackGroundView
    {
        [SerializeField] private Canvas _canvas;

        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _canvas.worldCamera = FindObjectOfType<Camera>();
        }
    }
}