using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class Checkpoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out CheckpointManager manager))
                    manager.Current = transform;
            }
        }
    }
}
