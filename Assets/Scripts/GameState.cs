using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public static class GameState
    {
        public static bool IsScripted { get; set; }

        public delegate void OnCutsceneEnter();
        public static event OnCutsceneEnter onCutsceneEnter;

        public delegate void OnCutsceneExit();
        public static event OnCutsceneExit onCutsceneExit;

        public static bool IsTrapped { get; set; }

        static GameState()
        {
            // Nothing for now
        }

        public static void ToggleCutscene()
        {
            if (IsScripted)
                onCutsceneExit?.Invoke();
            else
                onCutsceneEnter?.Invoke();

            IsScripted = !IsScripted;
        }
    }
}
