using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class HealthUI : MonoBehaviour
    {
        private PlayerManager player;

        private List<Animator> hearts;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();

            hearts = new List<Animator>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out Animator anim))
                    hearts.Add(anim);
            }
        }

        private void OnEnable()
        {
            player.onHit += DecreaseHealth;

            SetDefaultHealth();
        }

        private void OnDisable()
        {
            player.onHit -= DecreaseHealth;
        }

        private void SetDefaultHealth()
        {
            for (int i = 0; i < player.DefaultHealth; i++)
                hearts[i].SetBool("IsFilled", true);
        }

        private void IncreaseHealth()
        {
            // TODO: this lol
        }

        private void DecreaseHealth()
        {
            hearts[player.Health].SetBool("IsFilled", false);
        }
    }
}
