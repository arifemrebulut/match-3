using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agave
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;

        private List<T> _pooledObjects;
        private int _poolCapacity;
        private bool _initialized;

        public void InitializePool(int poolCapacity = 0)
        {
            if (poolCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(poolCapacity),"Pool capacity must be non-negative value.");
            }

            _poolCapacity = poolCapacity;

            _pooledObjects = new List<T>();

            for (int i = 0; i < poolCapacity; i++)
            {
                GameObject newObject = Instantiate(prefab.gameObject, transform);
                newObject.SetActive(false);
                
                _pooledObjects.Add(newObject.GetComponent<T>());
            }

            _initialized = true;
        }

        protected T GetPooledObject()
        {
            if (!_initialized)
            {
                InitializePool(1);
            }

            for (int i = 0; i < _poolCapacity; i++)
            {
                var pooledObject = _pooledObjects[i];

                if (pooledObject.isActiveAndEnabled) continue;
                
                return pooledObject;
            }

            var newObject = Instantiate(prefab.gameObject, transform);
            newObject.SetActive(false);
            _pooledObjects.Add(newObject.GetComponent<T>());

            return newObject.GetComponent<T>();
        }

        protected void ReturnObjectToPool(T objectToReturn)
        {
            if (objectToReturn == null)
            {
                return;
            }
            
            objectToReturn.gameObject.SetActive(false);

            if (!_initialized)
            {
                InitializePool();
                _pooledObjects.Add(objectToReturn);
            }
            
            _pooledObjects.Add(objectToReturn);
        }
    }
}