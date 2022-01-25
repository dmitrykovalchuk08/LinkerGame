using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration.DataStructures;
using ContentProvider;
using Pool.Implementation;
using Pool.Interfaces;
using UnityEngine;

namespace Game.Level.LevelVisual
{
    public class BlocksPooledProvider
    {
        private readonly Dictionary<BlockType, IObjectPool<BlockView>> pools;
        private readonly IVisualContentProvider contentProvider;

        public BlocksPooledProvider(IVisualContentProvider contentProvider)
        {
            this.contentProvider = contentProvider;
            pools = new Dictionary<BlockType, IObjectPool<BlockView>>();
        }

        public void CreatePool(BlockType blockType, int count)
        {
            if (pools.ContainsKey(blockType))
            {
                return;
            }

            var pool = new GameObjectPool<BlockView>();
            pool.InitializePool(count,
                () => contentProvider.GetBoardObject(blockType));
            pools.Add(blockType, pool);
        }

        public async Task<BlockView> GetBoardObject(BlockType blockType)
        {
            if (pools.ContainsKey(blockType))
            {
                return await pools[blockType].Get();
            }
            return null;
        }

        public void PutBack(BlockView tile)
        {
            if (pools.ContainsKey(tile.Type))
            {
                pools[tile.Type].Release(tile);
            }
            else
            {
                Object.Destroy(tile.gameObject);
            }
        }
    }
}