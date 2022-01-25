using UnityEngine;

namespace UI
{
    public abstract class GuiController : MonoBehaviour
    {
        [SerializeField] private string viewKey;
        public string Key => viewKey;
        public abstract void Show(object data);
        public abstract void Hide();
    }
}