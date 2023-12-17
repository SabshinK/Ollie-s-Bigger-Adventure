using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Circle
{
    public class DisableGravityField : MonoBehaviour
    {
        private InputAction gravityAction;
        private GravityFieldUI ui;

        [SerializeField] private bool enableEnterText;

        private void Awake()
        {
            gravityAction = InputHandler.GetAction("Toggle Gravity");
            ui = FindObjectOfType<GravityFieldUI>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (enableEnterText)
                    ui.SetStatus("Reverse Gravity Device Disabled");
                gravityAction.Disable();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ui.SetStatus("Reverse Gravity Device Enabled");
                gravityAction.Enable();
            }
        }
    }
}
