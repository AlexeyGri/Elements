using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Features.Levels.Components.Element.Models;
using InputSystem.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Views
{
    public class ElementView : MonoBehaviour, IElementView
    {
        private const int MsInSec = 1000;
        
        [Space] [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _moveSpeedS;

        private static readonly int IsAlive = Animator.StringToHash("IsAlive");

        private ElementModel _model;
        private int _destroyAnimationMs;
        private Directions _presetDirection;
        private Vector2 _presetTargetPosition;
        private int _presetOrder;

        public int Id { get; private set; }
        public int Order => _spriteRenderer.sortingOrder;
        public Vector2 Position { get; private set; }
        public bool IsLocked { get; private set; }
        public bool IsBusy { get; private set; }

        public void Initialize(ElementModel model)
        {
            _model = model;
            Id = model.Id;
            
            _animator.runtimeAnimatorController = model.AnimatorController;
            _spriteRenderer.sprite = model.Sprite;
            _spriteRenderer.enabled = true;
            
            if (Id < 0)
            {
                return;
            }
            
            var destroyAnimation = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(a => a.name.Contains("Destroy"));
            _destroyAnimationMs = (int)(destroyAnimation.length * MsInSec);
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
            Release();
            _transform.gameObject.SetActive(false);
        }

        public async UniTask PlayDestroyAsync(CancellationToken token)
        {
            Id = -1;
            
            Lock();

            var waitAnimation = UniTask.Delay(_destroyAnimationMs);
            SetAlive(false);

            await waitAnimation;
            if (token.IsCancellationRequested)
            {
                return;
            }
            
            _spriteRenderer.enabled = false;
        }

        public void TakeInBusiness()
        {
            IsBusy = true;
        }

        public void Lock()
        {
            IsLocked = Id > -1;
        }

        public void Release()
        {
            IsBusy = false;
            IsLocked = false;
        }

        public void PresetMoveData(Directions direction, Vector2 targetPosition, int order)
        {
            _presetDirection = direction;
            _presetTargetPosition = targetPosition;
            _presetOrder = order;
        }

        public UniTask MoveToPresetData(CancellationToken token)
        {
            return MoveTo(_presetDirection, _presetTargetPosition, _presetOrder, token);
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
            if (_model.Id < 0)
            {
                return;
            }
            
            _animator.SetBool(IsAlive, isAlive);
        }
    }
}