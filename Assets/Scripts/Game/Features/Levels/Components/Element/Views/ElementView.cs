using System.Drawing;
using Core.Views;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Features.Levels.Components.Element.Models;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Views
{
    public class ElementView : ViewBase, IElementView
    {
        [Space]
        [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private static readonly int IsAlive = Animator.StringToHash("IsAlive");

        private ElementModel _model;
        
        public int Id => _model.Id;
        public int Order => _spriteRenderer.sortingOrder;

        public void Initialize(ElementModel model)
        {
            _model = model;
            
            _animator.runtimeAnimatorController = model.AnimatorController;
            _spriteRenderer.sprite = model.Sprite;
        }

        public void Setup(System.Numerics.Vector3 position, float size, int order)
        {
            throw new System.NotImplementedException();
        }

        public void Setup(Vector2 position, float size, int order)
        {
            _transform.position = position;
            var center = _spriteRenderer.bounds.center;
            _spriteRenderer.bounds = new Bounds(center, new Vector2(size, size));
            _spriteRenderer.sortingOrder = order;
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

        public void ShowDestroy()
        {
            SetAlive(false);
        }

        public async void MoveTo(Directions direction, float target, int order)
        {
            switch (direction)
            {
                case Directions.Down:
                    _transform.DOLocalMoveY(-target, 1f).Play();
                    break;
                case Directions.Up:
                    _transform.DOLocalMoveY(target, 1f).Play();
                    break;
                case Directions.Left:
                    _transform.DOMoveX(target, 1f).Play();
                    break;
                case Directions.Right:
                    _transform.DOMoveX(-target, 1f).Play();
                    break;
            }

            await UniTask.Delay(500);

            _spriteRenderer.sortingOrder = order;
        }

        private void SetAlive(bool isAlive)
        {
            if (Id < 0)
            {
                return;
            }
            
            _animator.SetBool(IsAlive, isAlive);
        }
    }
}