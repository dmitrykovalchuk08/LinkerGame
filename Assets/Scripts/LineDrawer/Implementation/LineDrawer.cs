using LineDrawer.Interfaces;
using UnityEngine;

namespace LineDrawer.Implementation
{
    public class LineDrawer : AbstractLineDrawer
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private float scrollSpeed;

        public override void DrawLineToPosition(Vector3 pos)
        {
            var positionCount = line.positionCount;
            positionCount++;
            line.positionCount = positionCount;
            line.SetPosition(positionCount - 1, pos);
        }

        public override void RemoveLastLine()
        {
            if (line.positionCount > 0)
            {
                line.positionCount--;
            }
        }

        public override void ClearAll()
        {
            line.positionCount = 0;
        }

        private void Update()
        {
            var offset = Time.time * scrollSpeed;
            line.material.mainTextureOffset = new Vector2(offset % 1, 0);
        }
    }
}