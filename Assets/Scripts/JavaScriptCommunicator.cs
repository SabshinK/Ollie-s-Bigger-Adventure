using System.Runtime.InteropServices;
using UnityEngine;

namespace DamnGuys.Utilities
{
    public class JavaScriptCommunicator : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void TriggerIframeSwitch(int gameIndex);  // Pass an integer representing the game index

        public void SwitchToGame(int gameIndex)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TriggerIframeSwitch(gameIndex);  // Send the game index when switching
#else
            Debug.Log("Loading game " + gameIndex);
#endif
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
        private bool hasSwitched = false;

        void Update()
        {
            // Switch iframe only once when spacebar is pressed
            if (!hasSwitched && Input.GetKeyDown(KeyCode.T))
            {
                int gameIndex = 1;  // Example: You could choose which game index you want to pass
                TriggerIframeSwitchJS(gameIndex);
            }
        }
        
        private void TriggerIframeSwitchJS(int gameIndex)
        {
            if (hasSwitched) return;
            hasSwitched = true;
            TriggerIframeSwitch(gameIndex);  // Pass the game index to JavaScript
        }
#endif
    }
}