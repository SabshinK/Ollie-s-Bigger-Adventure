using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class CheckpointManager : MonoBehaviour
    {
        private Transform current;
        public Transform Current
        {
            get { return current; }
            set { current = value; }
        }
    }
}
