using System.Threading.Tasks;
using Configuration.DataStructures;
using UnityEngine;

namespace ContentProvider
{
    public interface IVisualContentProvider
    {
        Task<GameObject> GetBackgroundTile();
        Task<GameObject> GetBoardObject(BlockType type);
    }
}