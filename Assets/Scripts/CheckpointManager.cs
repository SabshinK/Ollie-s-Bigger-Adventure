using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private Checkpoint current;
        public Checkpoint Current
        {
            get { return current; }
            set { current = value; }
        }

        public void SetSpawn(GameObject obj)
        {
            obj.transform.position = current.transform.position;
        }
    }
}
