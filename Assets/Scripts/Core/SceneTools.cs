using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Utilities.Singleton;

namespace Utilities.Core
{
    /// <summary>
    /// Provide various common utility functions for scene management and tracking scene transition
    /// state.
    /// </summary>
    public static class SceneTools
    {
        /// <summary>
        /// Will be invoked when a scene transition starts. 
        /// Passes the index of the scene being transitioned to.
        /// </summary>
        public static Action<int> OnSceneTransitionStart;

        /// <summary>
        /// Returns true if the scene is currently transitioning.
        /// </summary>
        public static bool Transitioning = false;

        /// <summary>
        /// Returns true if there is a next scene to transition to.
        /// </summary>
        public static bool NextSceneExists => NextSceneIndex < SceneManager.sceneCountInBuildSettings;

        /// <summary>
        /// Returns the index of the next scene in the build settings.
        /// </summary>
        public static int NextSceneIndex => CurrentSceneIndex + 1;

        /// <summary>
        /// Will return the index of the current scene.
        /// </summary>
        public static int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// Will try and transition to the next scene in the build settings.
        /// </summary>
        public static IEnumerator TransitionToNextScene()
        {
            yield return TransitionToScene(NextSceneIndex % SceneManager.sceneCountInBuildSettings);
        }

        /// <summary>
        /// Will try and transition to the specified scene in the build settings.
        /// </summary>
        public static IEnumerator TransitionToScene(string sceneName)
        {
            yield return TransitionToScene(SceneManager.GetSceneByName(sceneName).buildIndex);
        }

        /// <summary>
        /// Will try and transition to the next scene in the build settings.
        /// </summary>
        public static IEnumerator TransitionToScene(int sceneIndex)
        {
            if (!Transitioning)
            {
                Transitioning = true;
                OnSceneTransitionStart?.Invoke(sceneIndex);
                yield return FadeToBlackSystem.TryPerformFadeAction(FadeToBlackSystem.FadeType.FadeIntoBlack, 1f);
                SceneManager.LoadScene(sceneIndex);
                Transitioning = false;
            }
        }
    }
}
