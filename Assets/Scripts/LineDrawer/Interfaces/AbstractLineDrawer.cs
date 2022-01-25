using UnityEngine;

namespace LineDrawer.Interfaces
{
    public abstract class AbstractLineDrawer : MonoBehaviour
    {
        public abstract void DrawLineToPosition(Vector3 pos);
        public abstract void RemoveLastLine();
        public abstract void ClearAll();
    }
}