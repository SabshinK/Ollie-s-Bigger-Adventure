using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("The maximum size of the object pool to pull from. Reduces overhead from creating and destroying gameObjects frequently.")]
        public int _maxPoolSize = 10;

        private Stack<GameObject> _inactive;

        private PrefabFactory _factory;
        public GameObject prefab;

        private void Awake()
        {
            _inactive = new Stack<GameObject>();
            _factory = new PrefabFactory();
            _factory.Prefab = prefab;
            _factory.Parent = transform;
        }

        /// <summary>
        /// Creates as many objects as are allowed by the max size, then starts cycling through old inactive objects.
        /// </summary>
        /// <param name="position">The starting position of the object.</param>
        /// <param name="rotation">The starting rotation of the object.</param>
        /// <returns>The spawned object.</returns>
        public GameObject Spawn(Vector3 position, Quaternion? rotation = null)
        {
            if (transform.childCount < _maxPoolSize)
                return _factory.CreateAt(position, rotation ?? Quaternion.identity);

            if (_inactive.Count > 0)
            {
                var obj = _inactive.Pop();

                obj.transform.position = position;
                obj.transform.rotation = rotation ?? Quaternion.identity;
                obj.SetActive(true);

                return obj;
            }
            else
                return null;
        }

        public void AddInactive(GameObject obj)
        {
            _inactive.Push(obj);
        }
    }
}
