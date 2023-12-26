using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Linq;

namespace TT
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        static Dictionary<string, Tuple<Transform, LinkedList<MonoBehaviour>>> _pools
            = new Dictionary<string, Tuple<Transform, LinkedList<MonoBehaviour>>>();

        public T GetObject<T>(string poolName) where T : MonoBehaviour
        {
            if (!_pools.ContainsKey(poolName))
            {
                return null;
            }

            LinkedList<MonoBehaviour> objects = _pools[poolName].Item2;

            foreach (MonoBehaviour obj in objects)
            {
                if (obj != null && !obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj as T;
                }
            }
            return null;
        }

        public Transform CreatePool<T>(string poolName, T prefab, int size) where T : MonoBehaviour
        {
            if (_pools.ContainsKey(poolName))
            {
                return _pools[poolName].Item1;
            }

            LinkedList<MonoBehaviour> objects = new LinkedList<MonoBehaviour>();

            Transform pool = new GameObject("[Pool]: " + poolName).transform;
            pool.position = Vector3.zero;
            for (int i = 0; i < size; ++i)
            {
                T newObj = GameObject.Instantiate(prefab, pool);
                newObj.gameObject.name = prefab.name;
                newObj.gameObject.SetActive(false);
                objects.AddLast(newObj);
            }

            _pools.Add(poolName, new Tuple<Transform, LinkedList<MonoBehaviour>>(pool, objects));

            return pool;
        }

        public bool AddMoreObject(string poolName, int amount)
        {
            if (!_pools.ContainsKey(poolName))
            {
                return false;
            }

            MonoBehaviour prefab = null;
            LinkedList<MonoBehaviour> objects = _pools[poolName].Item2;
            foreach(MonoBehaviour obj in objects)
            {
                if(obj != null)
                {
                    prefab = obj;
                    break;
                }
            }
            if(prefab == null)
            {
                return false;
            }

            Transform pool = _pools[poolName].Item1;
            for (int i = 0; i < amount; i++)
            {
                MonoBehaviour newObj = GameObject.Instantiate(prefab, pool);
                newObj.gameObject.name = prefab.name;
                newObj.gameObject.SetActive(false);
                objects.AddLast(newObj);
            }

            return true;
        }

        public void DestroyPool(string poolName)
        {
            if (!_pools.ContainsKey(poolName))
            {
                return;
            }

            Transform pool = _pools[poolName].Item1;
            GameObject.Destroy(pool.gameObject);
        }
    }
}
