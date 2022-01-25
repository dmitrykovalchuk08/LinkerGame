using System;
using System.Threading.Tasks;
using Configuration.DataStructures;
using ContentProvider;
using DG.Tweening;
using Pool;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class BlockView : LevelObject, IPooledItem
    {
        [SerializeField] private GameObject selection;
        [SerializeField] private BlockType type;
        public BlockType Type => type;

        private BoardCoordinates coordinates;
        private Sequence sequence;

        public void SetPosition(BoardCoordinates coords)
        {
            coordinates = coords;
            transform.position = CoordinatesProvider.BoardCoordinatesToWorld(coords);
        }

        public Task AnimateToPosition(BoardCoordinates coords, float delay = 0)
        {
            sequence?.Kill();

            sequence = DOTween.Sequence();
            if (delay > 0)
            {
                sequence.AppendInterval(delay);
            }

            var fallTime = (coords.Y - coordinates.Y + 1) * VisualData.BlockAnimationConfiguration.FallSpeed;
            sequence.Append(
                transform.DOMove(CoordinatesProvider.BoardCoordinatesToWorld(coords), fallTime)
                    .SetEase(VisualData.BlockAnimationConfiguration.FallCurve));

            sequence.Append(
                transform.DOScaleX(1.5f * VisualData.CellSize,
                        VisualData.BlockAnimationConfiguration.FallSpeed * 2)
                    .SetEase(VisualData.BlockAnimationConfiguration.BounceCurve));
            sequence.Join(
                transform.DOScaleY(.6f * VisualData.CellSize,
                        VisualData.BlockAnimationConfiguration.FallSpeed * 2)
                    .SetEase(VisualData.BlockAnimationConfiguration.BounceCurve));
            sequence.AppendCallback(() => { coordinates = coords; }
            );

            return sequence.AsyncWaitForCompletion();
        }

        public Task AnimateToDestroy()
        {
            sequence?.Kill();

            sequence = DOTween.Sequence();
            sequence.Append(
                transform.DOScale(0,
                        VisualData.BlockAnimationConfiguration.FallSpeed * 2)
                    .SetEase(Ease.Linear));
            return sequence.AsyncWaitForCompletion();
        }

        public void SetSelected(bool state)
        {
            selection.gameObject.SetActive(state);
        }

        public Task FallToDestroy(int fallToPos)
        {
            sequence?.Kill();

            sequence = DOTween.Sequence();
            sequence.Append(DOTween.Shake(
                    () => transform.position,
                    x => transform.position = x,
                    1f
                )
            );

            var fallTime = (fallToPos - coordinates.Y + 1) * VisualData.BlockAnimationConfiguration.FallSpeed;
            coordinates = new BoardCoordinates(coordinates.X, fallToPos);
            sequence.Append(
                transform.DOMove(CoordinatesProvider.BoardCoordinatesToWorld(coordinates), fallTime)
                    .SetEase(VisualData.BlockAnimationConfiguration.FallCurve));
            return sequence.AsyncWaitForCompletion();
        }

        public void ResetItem()
        {
            sequence?.Kill();
            transform.localScale = Vector3.one * VisualData.CellSize;
            selection.gameObject.SetActive(false);
            coordinates = null;
            var tr = transform;
            tr.parent = null;
            tr.position = Vector3.zero;
            CoordinatesProvider = null;
            VisualData = null;
        }
    }
}