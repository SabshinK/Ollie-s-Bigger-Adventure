using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class DisableGravityField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<ReverseGravityAbility>().DisableGravity();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<ReverseGravityAbility>().EnableGravity();
        }
    }
}
