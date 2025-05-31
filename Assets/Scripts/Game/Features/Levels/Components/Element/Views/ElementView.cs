using System.Threading;
using Core.Views;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Features.Levels.Components.Element.Models;
using InputSystem.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Views
{
    public class ElementView : ViewBase, IElementView
    {
        [Space] [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _moveSpeedS;

        private static readonly int IsAlive = Animator.StringToHash("IsAlive");

        private ElementModel _model;

        public int Id => _model.Id;
        public int Order => _spriteRenderer.sortingOrder;
        public Vector2 Position { get; private set; }
        public bool IsLocked { get; private set; }

        public void Initialize(ElementModel model)
        {
            _model = model;
            
            _animator.runtimeAnimatorController = model.AnimatorController;
            _spriteRenderer.sprite = model.Sprite;
        }

        public void Setup(Vector2 position, float size, int order)
        {
            _transform.position = position;
            Position = _transform.position;
            var center = _spriteRenderer.bounds.center;
            _spriteRenderer.bounds = new Bounds(center, new Vector2(size, size));
            _spriteRenderer.sortingOrder = order;

            Release();
        }

        public void Show()
        {
            _transform.gameObject.SetActive(true);
            SetAlive(true);
        }

        public void Hide()
        {
            _transform.gameObject.SetActive(false);
        }

        public void PlayDestroy()
        {
            Lock();
            SetAlive(false);
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Release()
        {
            IsLocked = false;
        }

        public async UniTask MoveTo(Directions direction, Vector2 targetPosition, int order, CancellationToken token)
        {
            Lock();
            
            _spriteRenderer.sortingOrder = order;
            Position = targetPosition;
            
            switch (direction)
            {
                case Directions.Down:
                case Directions.Up:
                    await _transform.DOLocalMoveY(targetPosition.y, _moveSpeedS).Play().ToUniTask(cancellationToken: token)
                        .SuppressCancellationThrow();
                    break;
                case Directions.Left:
                case Directions.Right:
                    await _transform.DOMoveX(targetPosition.x, _moveSpeedS).Play().ToUniTask(cancellationToken: token)
                        .SuppressCancellationThrow();
                    break;
            }
        }

        private void SetAlive(bool isAlive)
        {
            if (Id < 0)
            {
                return;
            }

            _animator.SetBool(IsAlive, isAlive);
            _spriteRenderer.enabled = isAlive;
        }
    }
}