using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Core;

namespace Circle
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
                StartCoroutine(SceneTools.TransitionToNextScene());
            }
        }
    }
}
