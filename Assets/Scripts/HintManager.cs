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
        private InputAction.CallbackContext? cachedContext;

        private int progress = 0;

        private void Awake()
        {
            moveAction = InputHandler.Inputs.Player.Horizontal;
            gravityAction = InputHandler.Inputs.Player.ToggleGravity;

            cachedContext = null;
        }

        private void OnEnable()
        {
            foreach (TriggerEventWrapper trigger in triggers)
                trigger.onTriggerEnter.AddListener(EnableHint);

            moveAction.performed += DisableHint;
            gravityAction.performed += DisableHint;
        }

        private void OnDisable()
        {
            foreach (TriggerEventWrapper trigger in triggers)
                trigger.onTriggerEnter.RemoveListener(EnableHint);

            moveAction.performed -= DisableHint;
            gravityAction.performed -= DisableHint;
        }

        private void EnableHint(Collider other)
        {
            animators[progress].SetBool("Enabled", true);

            StartCoroutine(CallCache());
        }

        private void DisableHint(InputAction.CallbackContext context)
        {
            // Don't do this part until there is a hint enabled
            if (animators[progress].GetBool("Enabled")) {
                switch (progress)
                {
                    case 0:
                        if (context.ReadValue<float>() > 0)
                        {
                            animators[progress].SetBool("Enabled", false);
                            progress++;
                        }
                        break;
                    case 1:
                        if (context.ReadValue<float>() < 0)
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
            else
            {
                /*
                 * It's possible for this function to get called before a hint is ready.
                 * In this case, the value should be stored for later so that DisableHint can be called again
                 */
                cachedContext = context;
            }
        }

        // Need a little bit of time before calling cached method otherwise it gets called too quickly
        private IEnumerator CallCache()
        {
            yield return new WaitForSeconds(0.5f);

            /* 
             * We need a check here to make sure the player isn't holding the next button. We don't
             * need to care what button is being pressed, just that one currently is
             */
            if (moveAction.IsPressed() && cachedContext != null)
            {
                DisableHint(cachedContext.Value);
                cachedContext = null;
            }
        }
    }
}
