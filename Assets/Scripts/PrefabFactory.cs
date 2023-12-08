using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class PrefabFactory
    {
        public GameObject Prefab { get; set; }
        public Transform Parent { get; set; }

        public GameObject CreateAt(Vector3 position, Quaternion rotation, string name = null)
        {
            var prefab = GameObject.Instantiate<GameObject>(Prefab, position, rotation, Parent);
            prefab.name = name ?? Prefab.name;
            return prefab;
        }
    }
}
