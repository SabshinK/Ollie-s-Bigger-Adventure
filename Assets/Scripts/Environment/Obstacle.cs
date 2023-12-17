using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Circle
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerManager player))
                    player.RegisterHit();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent(out PlayerManager player))
                    player.RegisterHit();
            }
        }
    }
}
