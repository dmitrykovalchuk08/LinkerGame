using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Pool.Interfaces;

namespace Pool.Implementation
{
    public class ObjectPool<T> : IObjectPool<T> where T : IPooledItem
    {
        protected Func<Task<T>> CreationDelegate;

        private readonly ConcurrentBag<T> items = new ConcurrentBag<T>();

        public void InitializePool(int limit, Func<Task<T>> creationDelegate)
        {
            CreationDelegate = creationDelegate;
            _ = InitPool(limit);
        }

        protected ObjectPool()
        {
        }

        public virtual void Release(T item)
        {
            item.ResetItem();
            items.Add(item);
        }

        public virtual async Task<T> Get()
        {
            if (items.TryTake(out var item))
            {
                return item;
            }
            else
            {
                T obj = await CreationDelegate.Invoke();
                return obj;
            }
        }

        protected async Task InitPool(int limit)
        {
            for (var i = 0; i < limit; i++)
            {
                items.Add(await CreationDelegate.Invoke());
            }
        }
    }
}