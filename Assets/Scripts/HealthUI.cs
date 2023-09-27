using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class HealthUI : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
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
                SetHeart(i, true);
        }

        private void IncreaseHealth()
        {
            
        }

        private void DecreaseHealth()
        {
            SetHeart(player.Health, false);
        }

        private void SetHeart(int index, bool isFilled)
        {
            Transform child = transform.GetChild(index);
            Animator anim = child?.GetComponent<Animator>();
            anim?.SetBool("IsFilled", isFilled);
        }
    }
}
