using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Controller;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.EventBus;
using Game.Features.Levels.Components.Element.Models;
using Game.Features.Levels.Components.Element.Views;
using Game.Features.Levels.Components.Grid.Views;
using Game.Features.Levels.Models;
using Game.Services;
using InputSystem;
using InputSystem.Models;
using UnityEngine;

namespace Game.Features.Levels
{
    public class LevelController : ControllerBase
    {
        private readonly IBundleProvider _bundleProvider;
        private readonly IInputManager _inputManager;
        private readonly IEventBus _eventBus;
        private readonly List<IElementView> _emptyElements = new();
        private readonly List<IElementView> _elements = new();

        private LevelModel _model;
        private GridView _gridView;
        private IElementView[] _elementViews;
        private IElementView _selectElement;

        private int ColumnCount => _model.GridModel.Columns;
        private int RowCount => _model.GridModel.Rows;

        public LevelController(IBundleProvider bundleProvider, IInputManager inputManager, IEventBus eventBus)
        {
            _bundleProvider = bundleProvider;
            _inputManager = inputManager;
            _eventBus = eventBus;
        }

        public void SetArgs(LevelModel model)
        {
            _model = model;
        }

        protected override async void OnStart()
        {
            var prefab = await _bundleProvider.LoadAssetAsync<GridView>(ResourcePaths.GridPath, Token);
            if (Token.IsCancellationRequested)
            {
                return;
            }

            _gridView = this.Instantiate(ControllerResources, prefab);
            _gridView.Setup(_model.GridModel);

            GetViews();
            CreateGrid();

            _gridView.PauseClicked += OnPauseClicked;
            _inputManager.TouchStartPosition += OnTouchStartPosition;
            _inputManager.Swipe += OnSwipe;
        }

        protected override void OnStop()
        {
            _inputManager.TouchStartPosition -= OnTouchStartPosition;
            _inputManager.Swipe -= OnSwipe;

            foreach (var elementView in _elementViews)
            {
                elementView.Hide();
            }
        }

        protected override void OnDispose()
        {
        }

        private void OnTouchStartPosition(Vector2 touchPosition)
        {
            var cell = _gridView.Cells.FirstOrDefault(c => c.InPoint(touchPosition));
            if (cell == null)
            {
                return;
            }

            _selectElement = _elementViews.FirstOrDefault(e => e.Position == cell.Position);
        }

        private async void OnSwipe(Directions direction)
        {
            if (_selectElement == null || _selectElement.IsLocked)
            {
                _selectElement = null;
                return;
            }

            if (!TryGetTargetIndex(direction, out var index))
            {
                _selectElement = null;
                return;
            }

            var target = _elementViews.FirstOrDefault(e => e.Order == index);
            if (target == null || target.IsLocked || target.Id == _selectElement.Id ||
                (index >= _selectElement.Order + ColumnCount && target.Id < 0))
            {
                _selectElement = null;
                return;
            }

            await SwitchElementsAsync(_selectElement, direction, target, Token);
            await NormalizationAsync(Token);
        }

        private void OnPauseClicked()
        {
            // save data
        }

        private async UniTask NormalizationAsync(CancellationToken token)
        {
            bool hasChanges;
            do
            {
                await ElementsFail(token);
                hasChanges = await HandleElementStates();
            } while (hasChanges);
        }

        private async UniTask ElementsFail(CancellationToken token)
        {
            var emptyCells = _elementViews.Where(e => e.Id < 0).ToArray();
            var tasks = new List<UniTask>();

            for (var i = 0; i < emptyCells.Length - 1; i++)
            {
                _emptyElements.Add(emptyCells[i]);

                var upperOrder = emptyCells[i].Order + ColumnCount;
                while (upperOrder < _elementViews.Length)
                {
                    var upperNeighbour = _elementViews.FirstOrDefault(e => e.Order == upperOrder);
                    if (upperNeighbour.Id < 0)
                    {
                        _emptyElements.Add(upperNeighbour);
                    }
                    else
                    {
                        _elements.Add(upperNeighbour);
                    }

                    upperOrder += ColumnCount;
                }

                for (var j = 0; j < _elements.Count; j++)
                {
                    var target = j >= _emptyElements.Count ? _emptyElements.Last() : _emptyElements[j];
                    tasks.Add(
                        SwitchElementsAsync(_elements[j], Directions.Down, target, token));
                }

                _emptyElements.Clear();
                _elements.Clear();
            }

            await tasks;
        }

        private async UniTask<bool> HandleElementStates()
        {
            var destroyResult = await TryDestroyElements();

            foreach (var elementView in _elementViews)
            {
                elementView.Release();
            }

            return destroyResult;
        }

        private async UniTask<bool> TryDestroyElements()
        {
            var lockElements = _elementViews.Where(e => e.IsLocked).ToArray();
            var destroyed = false;

            foreach (var element in lockElements)
            {
                if (element.InCombo)
                {
                    continue;
                }

                var combination = new CombinationElementModel(element, Directions.None);
                FindCombination(combination);

                var allCombinationElements = combination.AllElements;
                if (!IsValidCombination(allCombinationElements))
                {
                    continue;
                }

                var destroyAnimationTask = new List<UniTask>();
                allCombinationElements.ForEach(e => destroyAnimationTask.Add(e.PlayDestroy()));

                await destroyAnimationTask;

                destroyed = true;
            }

            return destroyed;
        }

        private void FindCombination(CombinationElementModel combination)
        {
            var rightElementOrder = combination.Element.Order + 1;
            if (combination.StartDirection != Directions.Right && rightElementOrder % ColumnCount != 0)
            {
                var element = _elementViews.FirstOrDefault(e => e.Order == rightElementOrder);
                if (element != null && combination.Element.Id == element.Id && !element.InCombo)
                {
                    AddChildCombination(element, Directions.Left);
                }
            }

            var downElementOrder = combination.Element.Order - ColumnCount;
            if (combination.StartDirection != Directions.Down && downElementOrder > -1)
            {
                var element = _elementViews.FirstOrDefault(e => e.Order == downElementOrder);
                if (element != null && combination.Element.Id == element.Id && !element.InCombo)
                {
                    AddChildCombination(element, Directions.Up);
                }
            }

            var leftElementOrder = combination.Element.Order - 1;
            if (combination.StartDirection != Directions.Left && leftElementOrder > -1 &&
                leftElementOrder % ColumnCount != ColumnCount - 1)
            {
                var element = _elementViews.FirstOrDefault(e => e.Order == leftElementOrder);
                if (element != null && combination.Element.Id == element.Id && !element.InCombo)
                {
                    AddChildCombination(element, Directions.Right);
                }
            }

            var upElementOrder = combination.Element.Order + ColumnCount;
            if (combination.StartDirection != Directions.Up && upElementOrder < _elementViews.Length)
            {
                var element = _elementViews.FirstOrDefault(e => e.Order == upElementOrder);
                if (element != null && combination.Element.Id == element.Id && !element.InCombo)
                {
                    AddChildCombination(element, Directions.Down);
                }
            }

            return;

            void AddChildCombination(IElementView element, Directions elementDirection)
            {
                var childCombination = new CombinationElementModel(element, elementDirection);
                combination.Neighbors.Add(childCombination);

                FindCombination(childCombination);
            }
        }

        private bool IsValidCombination(List<IElementView> elements)
        {
            if (elements.Count <= 2)
            {
                return false;
            }

            for (var i = 0; i <= elements.Count - 1; i++)
            {
                if ((elements.Any(e => e.Order == elements[i].Order + 1) &&
                     elements.Any(e => e.Order == elements[i].Order - 1))
                    || (elements.Any(e => e.Order == elements[i].Order + ColumnCount) &&
                        elements.Any(e => e.Order == elements[i].Order - ColumnCount)))
                {
                    return true;
                }
            }

            return false;
        }

        private void GetViews()
        {
            _elementViews = new IElementView[ColumnCount * RowCount];

            for (var i = 0; i < _model.ElementModels.Count; i++)
            {
                var operationResult =
                    _bundleProvider.TryGetInstanceFromPool<ElementView>(ControllerResources, ResourcePaths.ElementPath);
                if (operationResult.Item1)
                {
                    _elementViews[i] = operationResult.Item2;
                    _elementViews[i].Initialize(_model.ElementModels[i]);
                }
            }
        }

        private void CreateGrid()
        {
            var cells = _gridView.Cells;
            for (var i = 0; i < cells.Count; i++)
            {
                _elementViews[i].Setup(cells[i].Position, _gridView.CellsSize, i);
                _elementViews[i].Show();
            }
        }

        private bool TryGetTargetIndex(Directions direction, out int targetIndex)
        {
            targetIndex = -1;
            var selectIndex = _selectElement.Order;

            switch (direction)
            {
                case Directions.Up:
                    targetIndex = selectIndex + ColumnCount;
                    if (targetIndex >= _elementViews.Length)
                    {
                        return false;
                    }

                    break;
                case Directions.Down:
                    targetIndex = selectIndex - ColumnCount;
                    if (targetIndex < 0)
                    {
                        return false;
                    }

                    break;
                case Directions.Left:
                    if (selectIndex % ColumnCount == 0)
                    {
                        return false;
                    }

                    targetIndex = selectIndex - 1;
                    break;
                case Directions.Right:
                    if (selectIndex % ColumnCount == ColumnCount)
                    {
                        return false;
                    }

                    targetIndex = selectIndex + 1;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static Directions GetTargetDirections(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return Directions.Down;
                case Directions.Down:
                    return Directions.Up;
                case Directions.Left:
                    return Directions.Right;
                case Directions.Right:
                    return Directions.Left;
                default:
                    throw new ArgumentException($"{direction} does not exist.");
            }
        }

        private static UniTask SwitchElementsAsync(IElementView selectElement, Directions direction,
            IElementView target, CancellationToken token)
        {
            var selectOrder = selectElement.Order;
            var targetOrder = target.Order;
            var selectPosition = selectElement.Position;
            var targetPosition = target.Position;

            return UniTask.WhenAll(
                target.MoveTo(GetTargetDirections(direction), selectPosition, selectOrder, token),
                selectElement.MoveTo(direction, targetPosition, targetOrder, token));
        }
    }
}