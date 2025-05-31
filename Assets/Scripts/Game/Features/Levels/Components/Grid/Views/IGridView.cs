using System;
using UnityEngine;

namespace Game.Features.Levels.Components.Grid.Views
{
    public interface IGridView
    {
        GameObject GameObject { get; }
        event Action PauseClicked;
    }
}