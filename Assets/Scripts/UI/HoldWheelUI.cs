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
        [SerializeField] private int sceneToLoad = 0;

        [SerializeField] private UnityEvent onFilled;

        [SerializeField] private VideoPlayer vp;
        [SerializeField] private GameObject videoImg;

        private Image fill;
        private InputAction holdAction;        

        private void Awake()
        {
            holdAction = InputHandler.GetAction("Hold");
            fill = transform.GetChild(0).GetComponent<Image>();

            vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Version1_v01");
            vp.Play();
        }

        private void OnEnable()
        {
            InputHandler.Inputs.UI.Enable();

            holdAction.started += StartTimer;
            holdAction.canceled += CancelTimer;
            onFilled.AddListener(SetScene);

            vp.loopPointReached += EndVideo;
        }

        private void OnDisable()
        {
            InputHandler.Inputs.UI.Disable();

            holdAction.started -= StartTimer;
            holdAction.canceled -= CancelTimer;
            onFilled.RemoveListener(SetScene);

            vp.loopPointReached -= EndVideo;
        }

        private void EndVideo(VideoPlayer vp)
        {
            vp.gameObject.SetActive(false);
            vp.Stop();
            videoImg.SetActive(false);
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
