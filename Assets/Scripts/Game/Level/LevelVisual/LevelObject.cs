using Configuration.DataStructures;
using CoordinatesConvertor;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class LevelObject : MonoBehaviour
    {
        protected ICoordinatesProvider CoordinatesProvider;
        protected GameVisualData VisualData;

        public void Setup(
            Transform parent,
            ICoordinatesProvider coordinatesProvider,
            GameVisualData visualData)
        {
            VisualData = visualData;
            CoordinatesProvider = coordinatesProvider;
            transform.parent = parent;
        }
    }
}