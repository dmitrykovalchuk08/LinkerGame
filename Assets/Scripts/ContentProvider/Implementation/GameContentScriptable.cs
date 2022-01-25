using System;
using System.Collections.Generic;
using Configuration.DataStructures;
using UnityEngine;

namespace ContentProvider.Implementation
{
    [CreateAssetMenu(fileName = "GameContent", menuName = "Configuration/GameContent", order = 2)]
    public class GameContentScriptable : ScriptableObject
    {
        public GameObject woodenTile;

        public List<TileConfig> tiles;
    }

    [Serializable]
    public class TileConfig
    {
        public BlockType key;
        public GameObject model;
    }
}