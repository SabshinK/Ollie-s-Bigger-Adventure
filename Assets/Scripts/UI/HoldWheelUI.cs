using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Circle
{
    public class HoldWheelUI : MonoBehaviour
    {
        private Image fill;
        private InputAction holdAction;

        private Coroutine routine;

        private void Awake()
        {
            holdAction = InputHandler.GetAction("Hold");
            fill = transform.GetChild(0).GetComponent<Image>();
        }

        private void OnEnable()
        {
            InputHandler.Inputs.UI.Enable();

            holdAction.started += StartTimer;
            holdAction.canceled += CancelTimer;
        }

        private void OnDisable()
        {
            InputHandler.Inputs.UI.Disable();

            holdAction.started -= StartTimer;
            holdAction.canceled -= CancelTimer;
        }

        private void StartTimer(InputAction.CallbackContext context)
        {
            var interaction = context.interaction as HoldInteraction;
            routine = StartCoroutine(FillBar(interaction.duration));
        }

        private void CancelTimer(InputAction.CallbackContext context)
        {
            if (routine != null) StopCoroutine(routine);
            fill.fillAmount = 0;
        }

        private IEnumerator FillBar(float holdDuration)
        {
            float timer = 0;
            fill.fillAmount = 0;

            while (timer < holdDuration)
            {
                // Update the timer
                timer += Time.deltaTime;

                // Update UI
                fill.fillAmount = timer / holdDuration;

                yield return new WaitForEndOfFrame();
            }

            fill.fillAmount = 1;
        }
    }
}
