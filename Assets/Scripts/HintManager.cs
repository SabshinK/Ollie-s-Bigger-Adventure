using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Circle
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private TriggerEventWrapper[] triggers;
        [SerializeField] private Animator[] hints;

        private InputAction moveAction;
        private InputAction gravityAction;
    }
}
