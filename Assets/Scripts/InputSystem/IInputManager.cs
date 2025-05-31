using System;
using InputSystem.Models;
using UnityEngine;

namespace InputSystem
{
    public interface IInputManager
    {
        event Action<Vector2> TouchStartPosition;
        event Action<Directions> Swipe;
    }
}