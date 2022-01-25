using System;
using UnityEngine;

namespace Configuration.DataStructures
{
    [Serializable]
    public class GameVisualData
    {
        public Vector2 BoardCenter;
        public int CellSize;
        public BlockAnimationConfiguration BlockAnimationConfiguration;
    }

    [Serializable]
    public class BlockAnimationConfiguration
    {
        [Header("FALL")]
        public float FallSpeed;
        public float FallDelay;
        public AnimationCurve FallCurve;
        public AnimationCurve BounceCurve;
        
        [Header("IDLE")]
        public AnimationCurve idleCurve;
    }
}