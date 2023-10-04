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
            GameState.onCutsceneEnter += () => { anim?.SetBool("Cutscene", true); };
            GameState.onCutsceneExit += () => { anim?.SetBool("Cutscene", false); };
        }

        private void OnDisable()
        {
            GameState.onCutsceneEnter -= () => { anim?.SetBool("Cutscene", true); };
            GameState.onCutsceneExit -= () => { anim?.SetBool("Cutscene", false); };
        }
    }
}
