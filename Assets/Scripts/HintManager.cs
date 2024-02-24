using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Circle
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private TriggerEventWrapper[] triggers;
        [SerializeField] private Animator[] animators;

        private InputAction moveAction;
        private InputAction gravityAction;

        private int progress = 0;

        private void Awake()
        {
            moveAction = InputHandler.Inputs.Player.Horizontal;
            gravityAction = InputHandler.Inputs.Player.ToggleGravity;
        }

        private void OnEnable()
        {
            foreach (TriggerEventWrapper trigger in triggers)
                trigger.onTriggerEnter.AddListener(EnableHint);
            triggers[1].onTriggerExit.AddListener(DisableHintAlt);

            moveAction.performed += DisableHint;
            gravityAction.performed += DisableHint;
        }

        private void OnDisable()
        {
            foreach (TriggerEventWrapper trigger in triggers)
                trigger.onTriggerEnter.RemoveListener(EnableHint);
            triggers[1].onTriggerExit.RemoveListener(DisableHintAlt);

            moveAction.performed -= DisableHint;
            gravityAction.performed -= DisableHint;
        }

        private void EnableHint(Collider other)
        {
            animators[progress].SetBool("Enabled", true);
        }

        private void DisableHint(InputAction.CallbackContext context)
        {
            if (animators[progress].GetBool("Enabled")) {
                switch (progress)
                {
                    case 0:
                        if (context.control.name == "d")
                        {
                            animators[progress].SetBool("Enabled", false);
                            progress++;
                        }
                        break;
                    case 1:
                        if (context.control.name == "a")
                        {
                            animators[progress].SetBool("Enabled", false);
                            progress++;
                        }
                        break;
                    case 2:
                        if (context.action == gravityAction)
                            animators[progress].SetBool("Enabled", false);
                        break;
                }
            }
        }

        // This function catches the player if they speedrun past hint 2
        private void DisableHintAlt(Collider other)
        {
            if (animators[progress].GetBool("Enabled"))
            {
                animators[progress].SetBool("Enabled", false);
                progress++;
            }
        }
    }
}
