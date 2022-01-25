using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Pool.Implementation
{
    public class GameObjectPool<T> : ObjectPool<T> where T : MonoBehaviour, IPooledItem
    {
        private readonly Transform freeObjectsRoot;
        private Func<Task<GameObject>> goCreationDelegate;

        public GameObjectPool()
        {
            freeObjectsRoot = new GameObject($"pool<{typeof(T).Name}>").GetComponent<Transform>();
            CreationDelegate = IntermediateCreationDelegates;
        }

        public void InitializePool(int limit, Func<Task<GameObject>> creationDelegate)
        {
            goCreationDelegate = creationDelegate;
            _ = InitPool(limit);
        }

        private async Task<T> IntermediateCreationDelegates()
        {
            var tcs = new TaskCompletionSource<T>();
            var prefab = await goCreationDelegate();
            tcs.SetResult(prefab.GetComponent<T>());
            prefab.SetActive(false);
            prefab.transform.parent = freeObjectsRoot;
            return await tcs.Task;
        }

        public override async Task<T> Get()
        {
            var item = await base.Get();
            item.transform.parent = null;
            item.gameObject.SetActive(true);
            return item;
        }

        public override void Release(T item)
        {
            base.Release(item);
            item.transform.parent = freeObjectsRoot;
            item.gameObject.SetActive(false);
        }
    }
}