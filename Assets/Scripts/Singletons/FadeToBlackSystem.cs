using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities.Singleton
{
    /// <summary>
    /// Handles fading the screen to and from black. Based on the platform being used,
    /// different methods will need to be used to achieve the effect.
    /// <br/>
    /// <br/> <b>Non-VR</b>: Uses a full-screen Image component with a black color.
    /// <br/> <b>VR</b>: Uses a post-processing Volume with a Color Adjustments effect to change the color filter.
    /// </summary>
    public class FadeToBlackSystem : SingletonMonoBehavior<FadeToBlackSystem>
    {
        public enum FadeType
        {
            FadeIntoBlack,
            FadeOutOfBlack
        }

        [Header("Settings")]
        [Tooltip("If true, the scene will start with a full black screen.")]
        [SerializeField] private bool _startFullAlpha;
        [Tooltip("If true, the scene will automatically fade out to black at start of a scene.")]
        [SerializeField] private bool _autoFadeOut;

        [Header("Events")]
        [Tooltip("Will be invoked when finishes fading out of black")]
        [SerializeField] private UnityEvent _onFadeOutOfBlackFinished;

        [Header("Optional Settings")]
        [Tooltip("The image that will be used to fade in and out.")]
        [SerializeField] private Image _image;

        public UnityEvent OnFadeOutOfBlackFinished => _onFadeOutOfBlackFinished;

        /// <summary>
        /// Returns true if the screen is fully faded out.
        /// </summary>
        public static bool FadeOutComplete => Instance == null || Instance._image.color.a == 0;

        private void Start()
        {
            var alpha = _startFullAlpha ? 1 : 0;

            SetFadePercentage(alpha);
            SceneManager.sceneLoaded += CheckForAutoFadeOut;
            CheckForAutoFadeOut(new Scene(), LoadSceneMode.Single);
        }

        protected override void OnDestroy()
        {
            if (Instance == this) SetFadePercentage(0);
            base.OnDestroy();
            SceneManager.sceneLoaded -= CheckForAutoFadeOut;
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);
        }


        /// <summary>
        /// Will perform the corresponding fade action over a period of time equal to the specified duration. <br/>
        /// FadeType.FadeIntoBlack will fade the screen transparent => black. <br/>
        /// FadeType.FadeOutOfBlack will fade the screen black => transparent.
        /// </summary>
        public static IEnumerator TryPerformFadeAction(FadeType fadeType, float duration, Action callback = null)
        {
            if (Instance == null) yield break;

            var fadeIntoBlack = fadeType == FadeType.FadeIntoBlack;
            Instance.SetFadePercentage(fadeIntoBlack ? 0 : 1);

            var startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - startTime < duration)
            {
                var newAlpha = (Time.realtimeSinceStartup - startTime) / duration;
                Instance.SetFadePercentage(fadeIntoBlack ? newAlpha : 1 - newAlpha);
                yield return null;
            }

            Instance.SetFadePercentage(fadeIntoBlack ? 1 : 0);
            callback?.Invoke();
            if (!fadeIntoBlack) Instance.OnFadeOutOfBlackFinished?.Invoke();
        }

        private void CheckForAutoFadeOut(Scene _, LoadSceneMode __)
        {
            if (!_autoFadeOut) return;

            SetFadePercentage(1);
            StartCoroutine(HandleStartOfScene());
        }

        private void SetFadePercentage(float percentage)
        {
            if (_image != null)
            {
                var temp = _image.color;
                temp.a = percentage;
                _image.color = temp;
            }
        }

        private IEnumerator HandleStartOfScene()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(TryPerformFadeAction(FadeType.FadeOutOfBlack, 1.5f));
        }
    }
}
