using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Core;

namespace Circle
{
    public class StartScreen : MonoBehaviour
    {
        public void PlayGame()
        {
            StartCoroutine(SceneTools.TransitionToNextScene());
        }

        public void QuitGame()
        {
//#if UNITY_EDITOR
//            if (EditorApplication.isPlaying)
//                EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
        }
    }
}
