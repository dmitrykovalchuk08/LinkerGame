using Configuration.DataStructures;
using UnityEngine;

namespace CoordinatesConvertor
{
    public interface ICoordinatesProvider
    {
        void UpdateOffsetForBoard(int sizeX, int sizeY);
        Vector2 BoardCoordinatesToWorld(BoardCoordinates coordinates);
        BoardCoordinates WorldCoordinatesToBoard(Vector2 coordinates);
    }
}