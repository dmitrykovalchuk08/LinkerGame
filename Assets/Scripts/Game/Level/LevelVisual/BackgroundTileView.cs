using Configuration.DataStructures;
using Pool;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class BackgroundTileView : LevelObject, IPooledItem
    {
        public void SetPosition(BoardCoordinates coord)
        {
            transform.position = CoordinatesProvider.BoardCoordinatesToWorld(coord);
            name = $"[{coord.X}:{coord.Y}]";
        }

        public void ResetItem()
        {
            var tr = transform;
            tr.parent = null;
            tr.position = Vector3.zero;
            name = $"[-:-]";
            CoordinatesProvider = null;
            VisualData = null;
        }
    }
}