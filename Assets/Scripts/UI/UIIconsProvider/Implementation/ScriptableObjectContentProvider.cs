using UnityEngine;

namespace UI.UIIconsProvider.Implementation
{
    public class ScriptableObjectIconsProvider : IIconsProvider
    {
        private static ScriptableObjectIconsProvider instance;
        private readonly GameIconsScriptable scriptableObject;

        public static ScriptableObjectIconsProvider GetInstance()
        {
            return instance ?? (instance = new ScriptableObjectIconsProvider());
        }

        private ScriptableObjectIconsProvider()
        {
            scriptableObject = Resources.Load<GameIconsScriptable>("UIContent");
        }

        public Sprite GetIcon(string key)
        {
            var iconData = scriptableObject.sprites.Find(s => s.key == key);
            return iconData?.icon;
        }
    }
}