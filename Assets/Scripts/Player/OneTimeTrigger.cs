using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class OneTimeTrigger : MonoBehaviour
    {
        [SerializeField] private bool disableOnExit = false;

        private BoxCollider col;

        private void Awake()
        {
            col = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!disableOnExit)
                col.enabled = false;
        }

        private void OnTriggerExit(Collider other)
        {
            col.enabled = false;
        }
    }
}
