using System;
using System.Threading.Tasks;

namespace Pool.Interfaces
{
    public interface IObjectPool<T> where T : IPooledItem
    {
        void Release(T item);
        Task<T> Get();
        void InitializePool(int limit, Func<Task<T>> creationDelegate);
    }
}