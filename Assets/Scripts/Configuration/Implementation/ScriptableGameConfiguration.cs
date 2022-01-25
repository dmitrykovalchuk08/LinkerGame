using System.Collections.Generic;
using Configuration.DataStructures;
using UnityEngine;

namespace Configuration.Implementation
{
    [CreateAssetMenu(fileName = "RootConfig", menuName = "Configuration/GameConfiguration", order = 1)]
    public class ScriptableGameConfiguration : ScriptableObject
    {
        public LevelsData LevelData;
        public GameVisualData VisualConfig;

        private void OnEnable()
        {
            if (LevelData == null)
            {
                LevelData = new LevelsData()
                {
                    Data = new List<LevelConfiguration>()
                };
            }

            if (VisualConfig == null)
            {
                VisualConfig = new GameVisualData
                {
                    BoardCenter = Vector2.zero,
                    CellSize = 10
                };
            }
        }
    }
}