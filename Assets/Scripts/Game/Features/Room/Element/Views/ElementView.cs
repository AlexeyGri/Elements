using DG.Tweening;
using Game.Features.Room.Cell.Models;
using UnityEngine;

namespace Game.Features.Room.Element.Views
{
    public class ElementView : MonoBehaviour, IElementView
    {
        [SerializeField] private int _id;
        [Space]
        [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;

        private Tween _moveDown;
        private Tween _moveUp;
        private Tween _moveLeft;
        private Tween _moveRight;

        private static readonly int IsAlive = Animator.StringToHash("IsAlive");

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

        public void MoveTo(Directions direction, float target)
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
        }
    }
}