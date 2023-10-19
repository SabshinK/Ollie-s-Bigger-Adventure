using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class CutsceneUI : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameState.onCutsceneEnter += EnterCutscene;
            GameState.onCutsceneExit += ExitCutscene;
        }

        private void OnDisable()
        {
            GameState.onCutsceneEnter -= EnterCutscene;
            GameState.onCutsceneExit -= ExitCutscene;
        }

        private void EnterCutscene()
        {
            anim?.SetBool("Cutscene", true);
        }

        private void ExitCutscene()
        {
            anim?.SetBool("Cutscene", false);
        }
    }
}
