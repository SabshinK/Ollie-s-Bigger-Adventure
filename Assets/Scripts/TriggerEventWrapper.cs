using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Circle
{
    /// <summary>
    /// A simple script that exposes the trigger events of a certain object
    /// </summary>
    public class TriggerEventWrapper : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnter;
        public UnityEvent<Collider> onTriggerStay;
        public UnityEvent<Collider> onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStay.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke(other);
        }
    }
}
