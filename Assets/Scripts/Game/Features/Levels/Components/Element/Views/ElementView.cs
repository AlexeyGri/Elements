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

        public void Setup(ElementModel model, int order)
        {
            _model = model;
            
            _spriteRenderer.sortingOrder = order;
            
            _animator.runtimeAnimatorController = model.AnimatorController;
            _spriteRenderer.sprite = model.Sprite;
        }
        
        public void Show()
        {
            _animator.SetBool(IsAlive, true);
            _transform.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _transform.gameObject.SetActive(false);
        }

        public void ShowDestroy()
        {
            _animator.SetBool(IsAlive, false);
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
    }
}