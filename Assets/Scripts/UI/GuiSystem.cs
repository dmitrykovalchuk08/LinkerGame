using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class GuiSystem : MonoBehaviour
    {
        private static GuiSystem instance;
        private readonly Dictionary<string, GuiController> views = new Dictionary<string, GuiController>();

        public static void ShowView(string key, object data = null)
        {
            if (instance != null)
            {
                instance.ShowViewInternal(key, data);
            }
        }

        public static void HideView(string key)
        {
            if (instance != null)
            {
                instance.HideViewInternal(key);
            }
        }

        private void Awake()
        {
            instance = this;
            var list = transform.GetComponentsInChildren<GuiController>(true);
            foreach (var view in list)
            {
                views[view.Key] = view;
            }
        }

        private void ShowViewInternal(string key, object data)
        {
            if (views.ContainsKey(key))
            {
                views[key].Show(data);
            }
            else
            {
                Debug.LogError($"There is No registered view for {key}");
            }
        }

        private void HideViewInternal(string key)
        {
            if (views.ContainsKey(key))
            {
                views[key].Hide();
            }
            else
            {
                Debug.LogError($"There is No registered view for {key}");
            }
        }
    }
}