using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.Circle
{
    public class JavaScriptCommunicator : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ExitGame();  // Pass an integer representing the game index

        public void CueExitGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ExitGame();  // Send the game index when switching
#else
            Debug.Log("Exit game");
#endif
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
        private bool hasSwitched = false;

        void Update()
        {
            // Switch iframe only once when spacebar is pressed
            if (!hasSwitched && Input.GetKeyDown(KeyCode.Space))
            {
                hasSwitched = true;
                CueExitGame();
            }
        }
#endif
    }
}