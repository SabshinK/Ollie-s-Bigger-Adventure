using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Circle
{
    public class StartScreen : MonoBehaviour
    {
       
       public void PlayGame()
       {
            SceneManager.LoadSceneAsync(1);
       }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
