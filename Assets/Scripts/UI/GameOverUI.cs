using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Circle
{
    public class GameOverUI : MonoBehaviour
    {
        private PlayerManager manager;
        private Animator animator;

        private void Awake()
        {
            manager = FindObjectOfType<PlayerManager>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            manager.onDeath += Enable;
        }

        private void OnDisable()
        {
            manager.onDeath -= Enable;
        }

        private void Enable()
        {
            animator.SetTrigger("Game Over");
        }
    }
}
