using UnityEngine;
using UnityEngine.Pool;

namespace Mototaxi.Utils
{
    /// <summary>
    /// Wrapper for Unity Pool
    /// </summary>
    /// <typeparam name="T">Prefab component to pool</typeparam>
    public class ObjectPoolSc<T> where T : Component
    {
        private readonly IObjectPool<T> pool;
        private readonly T prefab;
        private readonly Transform container;

        public ObjectPoolSc(T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20)
        {
            this.prefab = prefab;
            container = parent;

            pool = new ObjectPool<T>(
                CreatePooledItem,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true, // collectionCheck
                defaultCapacity,
                maxSize
            );
        }

        public T Get() => pool.Get();
        public void Release(T item) => pool.Release(item);

        private T CreatePooledItem()
        {
            T instance = Object.Instantiate(prefab, container);
            return instance;
        }

        private void OnTakeFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnReturnedToPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(T item)
        {
            Object.Destroy(item.gameObject);
        }
    }
}
