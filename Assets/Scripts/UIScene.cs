using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Circle
{
    public class UIScene : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private InputActionMap ui;
        private InputActionMap player;

        private void Awake()
        {
            ui = InputHandler.Inputs.UI;
            player = InputHandler.Inputs.Player;
        }

        private void OnEnable()
        {
            player.Disable();
            ui.Enable();
        }

        private void OnDisable()
        {
            ui.Disable();
            player.Enable();
        }

        private void Start()
        {
            startButton.Select();
        }
    }
}
