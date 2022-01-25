using Configuration.DataStructures;
using Unity.Mathematics;
using UnityEngine;

namespace CoordinatesConvertor
{
    public class CoordinatesProvider : ICoordinatesProvider
    {
        private const float sensivity = .8f;

        private GameVisualData visualData;
        private Vector2 initialOffset;

        public CoordinatesProvider(GameVisualData gameVisualData)
        {
            visualData = gameVisualData;
        }

        public void UpdateOffsetForBoard(int sizeX, int sizeY)
        {
            initialOffset = new Vector2(
                (-sizeX / 2f + .5f) * visualData.CellSize,
                (sizeY / 2f - .5f) * visualData.CellSize
            );
        }

        public Vector2 BoardCoordinatesToWorld(BoardCoordinates coordinates)
        {
            return new Vector3(
                initialOffset.x + coordinates.X * visualData.CellSize,
                initialOffset.y - coordinates.Y * visualData.CellSize,
                0
            );
        }

        public BoardCoordinates WorldCoordinatesToBoard(Vector2 coordinates)
        {
            var fx = (coordinates - initialOffset).x / visualData.CellSize;
            var fy = (initialOffset - coordinates).y / visualData.CellSize;

            var rx = Mathf.RoundToInt(fx);
            var ry = Mathf.RoundToInt(fy);

            var dx = fx - rx;
            var dy = fy - ry;

            if (math.abs(dx) > sensivity / 2f ||
                math.abs(dy) > sensivity / 2f)
            {
                return null;
            }

            return new BoardCoordinates(rx, ry);
        }
    }
}