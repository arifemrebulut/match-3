using System;
using DG.Tweening;
using UnityEngine;

namespace Agave
{
    public class SwapController : MonoBehaviour, IService
    {
        private Drop[] _selectedDrops = new Drop[2];

        private DropBoard _dropBoard;

        private void Start()
        {
            _dropBoard = ServiceLocator.Get<DropBoard>();
        }

        public void SelectFirst(Drop dropToSelect)
        {
            _selectedDrops[0] = dropToSelect;
        }

        public void SelectSecond(Drop dropToSelect)
        {
            _selectedDrops[1] = dropToSelect;

            var canSwap =
                _selectedDrops[0] != null
                && _selectedDrops[1] != null
                && !_selectedDrops[0].IsMoving
                && !_selectedDrops[1].IsMoving
                && _selectedDrops[0] != _selectedDrops[1];

            if (!canSwap)
            {
                return;
            }

            if (SelectedDropsAreAdjacent())
            {
                TrySwap();
            }

            SelectFirst(null);
        }

        private void TrySwap()
        {
            var firstDrop = _selectedDrops[0];
            var secondDrop = _selectedDrops[1];

            var firstPosition = firstDrop.transform.position;
            var secondPosition = secondDrop.transform.position;

            _dropBoard.SwapItemsAt((int)firstPosition.x, (int)firstPosition.y,
                (int)secondPosition.x, (int)secondPosition.y);

            PlaySwapAnimation(firstDrop, secondDrop,
                () =>
                {
                    bool anyMatchOccured = false;

                    if (_dropBoard.IsPartOfMatch(firstDrop))
                    {
                        anyMatchOccured = true;

                        _dropBoard.ResolveMatch();
                    }

                    if (_dropBoard.IsPartOfMatch(secondDrop))
                    {
                        anyMatchOccured = true;

                        _dropBoard.ResolveMatch();
                    }

                    if (anyMatchOccured)
                    {
                        return;
                    }

                    _dropBoard.SwapItemsAt((int)firstPosition.x, (int)firstPosition.y,
                        (int)secondPosition.x, (int)secondPosition.y);

                    PlaySwapAnimation(firstDrop, secondDrop);
                });
        }

        private void PlaySwapAnimation(Drop firstDrop, Drop secondDrop, Action onComplete = null)
        {
            firstDrop.IsMoving = true;
            secondDrop.IsMoving = true;

            var temp = firstDrop.transform.position;

            var sequence = DOTween.Sequence();

            sequence.Join(firstDrop.transform.DOMove(secondDrop.transform.position, 0.2f))
                .Join(secondDrop.transform.DOMove(temp, 0.2f));

            sequence.OnComplete(() =>
            {
                firstDrop.IsMoving = false;
                secondDrop.IsMoving = false;

                onComplete?.Invoke();
            });
        }

        private bool SelectedDropsAreAdjacent()
        {
            var firstPosition = (Vector2)_selectedDrops[0].transform.position;
            var secondPosition = (Vector2)_selectedDrops[1].transform.position;

            if (firstPosition.x == secondPosition.x
                && Mathf.Abs(firstPosition.y - secondPosition.y) == 1)
            {
                return true;
            }

            if (firstPosition.y == secondPosition.y
                && Mathf.Abs(firstPosition.x - secondPosition.x) == 1)
            {
                return true;
            }

            return false;
        }
    }
}