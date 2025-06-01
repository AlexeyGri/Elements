using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Levels.Components.Element.Models;
using InputSystem.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Views
{
    public interface IElementView
    {
        public int Id { get; }
        int Order { get; }
        Vector2 Position { get; }
        bool IsLocked { get; }
        bool InCombo { get; }

        void Initialize(ElementModel model);
        void Setup(Vector2 position, float size, int order);
        void Show();
        void Hide();
        void TakeInCombo();
        void Lock();
        void Release();
        UniTask PlayDestroy();
        UniTask MoveTo(Directions direction, Vector2 targetPosition, int order, CancellationToken token);
    }
}