using System;
using System.Threading.Tasks;
using Configuration.DataStructures;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ContentProvider.Implementation
{
    public class ScriptableObjectContentProvider : IVisualContentProvider
    {
        private GameContentScriptable scriptableObject;

        public ScriptableObjectContentProvider()
        {
            scriptableObject = Resources.Load<GameContentScriptable>("GameContent");
        }

        public Task<GameObject> GetBackgroundTile()
        {
            var tcs = new TaskCompletionSource<GameObject>();
            var prefab = scriptableObject.woodenTile;
            tcs.SetResult(Object.Instantiate(prefab));
            return tcs.Task;
        }

        public Task<GameObject> GetBoardObject(BlockType type)
        {
            var tcs = new TaskCompletionSource<GameObject>();
            var template = scriptableObject.tiles.Find(t => t.key == type);
            if (template == null)
            {
                tcs.SetException(new Exception($"No config found for {type}"));
                return tcs.Task;
            }

            var prefab = template.model;
            tcs.SetResult(Object.Instantiate(prefab));
            return tcs.Task;
        }
    }
}