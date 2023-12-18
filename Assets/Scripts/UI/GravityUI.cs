using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Circle
{
    public class GravityUI : MonoBehaviour
    {
        private Image fill;
        private InputAction gravityAction;

        private Animator anim;

        private bool shouldStart = false;

        private void Awake()
        {
            gravityAction = InputHandler.GetAction("Toggle Gravity");
            fill = transform.GetChild(0).GetComponent<Image>();
            anim = GetComponent<Animator>();

            anim.SetBool("Enabled", false);

            StartCoroutine(Debounce());
        }

        private void OnEnable()
        {
            gravityAction.started += StartTimer;
            gravityAction.canceled += CancelTimer;
        }

        private void OnDisable()
        {
            gravityAction.started -= StartTimer;
            gravityAction.canceled -= CancelTimer;
        }

        private void StartTimer(InputAction.CallbackContext context)
        {
            if (shouldStart)
            {
                var interaction = context.interaction as HoldInteraction;
                StartCoroutine(ChargeGravity(interaction.duration));
            }
        }

        private void CancelTimer(InputAction.CallbackContext context)
        {
            //Debug.Log("Cancelled");
            StopAllCoroutines();
            anim.SetBool("Enabled", false);
        }

        private IEnumerator ChargeGravity(float holdDuration)
        {
            float timer = 0;
            fill.fillAmount = 0;
            anim.SetBool("Enabled", true);

            while (timer < holdDuration)
            {
                // Update the timer
                timer += Time.deltaTime;

                // Update UI
                fill.fillAmount = timer / holdDuration;

                yield return new WaitForEndOfFrame();
            }

            fill.fillAmount = 1;
            anim.SetBool("Enabled", false);
        }

        private IEnumerator Debounce()
        {
            yield return new WaitForSecondsRealtime(1f);

            shouldStart = true;
        }
    }
}
