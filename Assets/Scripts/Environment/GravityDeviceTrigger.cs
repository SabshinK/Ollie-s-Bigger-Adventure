using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Circle
{
    public class GravityDeviceTrigger : MonoBehaviour
    {
        private InputAction gravityAction;
        private GravityFieldUI ui;

        private void Awake()
        {
            gravityAction = InputHandler.GetAction("Toggle Gravity");
            gravityAction.Disable();

            ui = FindObjectOfType<GravityFieldUI>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ui.SetStatus("Reverse Gravity Device Acquired");
                gravityAction.Enable();
            }
        }
    }
}
