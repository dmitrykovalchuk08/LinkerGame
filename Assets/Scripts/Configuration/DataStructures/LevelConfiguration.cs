using System;
using System.Collections.Generic;

namespace Configuration.DataStructures
{
    [Serializable]
    public class LevelsData
    {
        public List<LevelConfiguration> Data;
    }

    [Serializable]
    public class LevelConfiguration
    {
        public int number;
        public int BoardWidth;
        public int BoardHeight;
        public int Moves; 
        public List<LevelCondition> WinConditions;
        public List<ObjectAppearData> ObjectProbabilities;
    }

    [Serializable]
    public class ObjectAppearData
    {
        public BlockType type;
        public float Probability;
    }

    [Serializable]
    public class LevelCondition
    {
        public ActionType ActionType;
        public BlockType BlockType;
        public int Quantity;
    }

    public enum BlockType
    {
        None = -1,
        Any = 0,
        Yellow = 1,
        Red = 2,
        Pink = 3,
        Green = 4,
        Blue = 5
    }

    public enum ActionType
    {
        CollectPoints,
        MovesLimit
    }
}