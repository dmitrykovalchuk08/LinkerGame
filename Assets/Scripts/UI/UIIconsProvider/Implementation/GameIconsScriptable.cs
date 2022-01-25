using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.UIIconsProvider.Implementation
{
    [CreateAssetMenu(fileName = "UIContent", menuName = "Configuration/UIContent", order = 3)]
    public class GameIconsScriptable : ScriptableObject
    {
        public List<IconConfig> sprites;
    }

    [Serializable]
    public class IconConfig
    {
        public string key;
        public Sprite icon;
    }
}