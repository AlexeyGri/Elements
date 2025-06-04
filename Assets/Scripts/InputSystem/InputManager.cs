using System;
using InputSystem.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : IInputManager, IDisposable
    {
        private readonly TouchScreenInput _input;
        private readonly Camera _camera;

        public event Action<Vector2> TouchStartPosition = delegate { };
        public event Action<Directions> Swipe = delegate { };

        private Vector2 _touchPosition;
        
        public InputManager()
        {
            _camera = Camera.main;
            
            _input = new TouchScreenInput();

            _input.Touch.TouchInput.performed += OnTouchStarted;
            _input.Touch.SwipeInput.performed += OnSwipe;
            
            _input.Enable();
        }
        
        public void Dispose()
        {
            _input.Disable();
            
            _input.Touch.TouchInput.performed -= OnTouchStarted;
            _input.Touch.SwipeInput.performed -= OnSwipe;
            
            _input.Dispose();
        }

        private void OnTouchStarted(InputAction.CallbackContext e)
        {
            _touchPosition = e.ReadValue<Vector2>();
            _touchPosition = _camera.ScreenToWorldPoint(_touchPosition);

            TouchStartPosition.Invoke(_touchPosition);
        }
        
        private void OnSwipe(InputAction.CallbackContext e)
        {
            var direction = e.ReadValue<Vector2>().normalized;
            
            if (direction == Vector2.up)
            {
                Swipe.Invoke(Directions.Up);
            }
            else if (direction == Vector2.down)
            {
                Swipe.Invoke(Directions.Down);
            }
            else if (direction == Vector2.right)
            {
                Swipe.Invoke(Directions.Right);
            }
            else if (direction == Vector2.left)
            {
                Swipe.Invoke(Directions.Left);
            }
        }
    }
}