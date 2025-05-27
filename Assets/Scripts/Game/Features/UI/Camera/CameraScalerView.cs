using UnityEngine;

namespace Views
{
    public class CameraScalerView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private int _targetWidth;
        [SerializeField] private float _pixelToUnits = 100;
        
        public void Reset()
        {
            _camera = GetComponent<Camera>();
        }

        public void OnEnable()
        {
            Scale(new Vector2(Screen.width, Screen.height));
        }

        public void Scale(Vector2 resolution)
        {
            var targetHeight =
                Mathf.RoundToInt(_targetWidth / resolution.x * resolution.y);

            _camera.orthographicSize = targetHeight / _pixelToUnits / 2;

            _camera.ResetAspect();

            _camera.aspect = (float)_targetWidth / targetHeight;
        }
    }
}