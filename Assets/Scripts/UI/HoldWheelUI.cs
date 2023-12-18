using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Circle
{
    public class HoldWheelUI : MonoBehaviour
    {
        private Image fill;
        private InputAction holdAction;

        [SerializeField] private int sceneToLoad = 0;

        [SerializeField] private UnityEvent onFilled;

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
            onFilled.AddListener(SetScene);
        }

        private void OnDisable()
        {
            InputHandler.Inputs.UI.Disable();

            holdAction.started -= StartTimer;
            holdAction.canceled -= CancelTimer;
            onFilled.RemoveListener(SetScene);
        }

        private void StartTimer(InputAction.CallbackContext context)
        {
            var interaction = context.interaction as HoldInteraction;
            StartCoroutine(FillBar(interaction.duration));
        }

        private void CancelTimer(InputAction.CallbackContext context)
        {
            StopAllCoroutines();
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

            onFilled?.Invoke();
        }

        private void SetScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
