using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    internal sealed class ViewServices
    {
        private readonly Dictionary<string, ObjectPool> _viewCache = new Dictionary<string, ObjectPool>(15);

        public Dictionary<string, ObjectPool> ViewCache => _viewCache;

        public event Action OnViewDestroy;
        
        public GameObject Instantiate(GameObject prefab)
        {
            GameObject gameObject;
            if(!ViewCache.TryGetValue(prefab.name, out ObjectPool viewPool))
            {
                viewPool = new ObjectPool(prefab);
                ViewCache[prefab.name] = viewPool;
            }

            gameObject = viewPool.Pop();
            return gameObject;
        }

        public void Destroy(GameObject value)
        {
            OnViewDestroy?.Invoke();
            ViewCache[value.name].Push(value);
        }
    }
}