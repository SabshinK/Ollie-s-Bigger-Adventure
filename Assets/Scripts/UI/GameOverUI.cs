using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Circle
{
    public class GameOverUI : MonoBehaviour
    {
        private PlayerManager manager;
        private TMP_Text text;

        private void Awake()
        {
            manager = FindObjectOfType<PlayerManager>();
            text = GetComponent<TMP_Text>();
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
            text.enabled = true;
        }
    }
}
