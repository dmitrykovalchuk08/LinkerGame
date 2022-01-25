using UnityEngine;

namespace UI.UIIconsProvider
{
    public interface IIconsProvider
    {
        Sprite GetIcon(string key);
    }
}