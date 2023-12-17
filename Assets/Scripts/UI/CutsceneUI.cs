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
            GameManager.onCutsceneEnter += EnterCutscene;
            GameManager.onCutsceneExit += ExitCutscene;
        }

        private void OnDisable()
        {
            GameManager.onCutsceneEnter -= EnterCutscene;
            GameManager.onCutsceneExit -= ExitCutscene;
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
