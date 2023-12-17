using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public static class GameManager
    {
        public static bool IsScripted { get; set; }

        public delegate void OnCutsceneEnter();
        public static event OnCutsceneEnter onCutsceneEnter;

        public delegate void OnCutsceneExit();
        public static event OnCutsceneExit onCutsceneExit;

        static GameManager()
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

    public enum GameState
    {
        Playing,
        Scripted,
        UI
    }
}
