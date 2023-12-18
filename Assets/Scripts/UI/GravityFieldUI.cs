using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Circle
{
    public class GravityFieldUI : MonoBehaviour
    {
        private TMP_Text text;
        private Animator anim;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            anim = GetComponent<Animator>();
        }

        public void SetStatus(string status)
        {
            text.text = status;
            anim.SetTrigger("Gravity Trigger");
        }
    }
}
