using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("The maximum size of the object pool to pull from. Reduces overhead from creating and destroying gameObjects frequently.")]
        [SerializeField] private int maxPoolSize = 10;

        [SerializeField] private GameObject prefab;

        private List<GameObject> pool;

        private void Awake()
        {
            pool = new List<GameObject>();
        }

        private void Start()
        {
            // Instantiate pooled objects
            for (int i = 0; i < maxPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            // Grab the first inactive object found. If there aren't any, return null
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }
            }
            return null;
        }
    }
}
