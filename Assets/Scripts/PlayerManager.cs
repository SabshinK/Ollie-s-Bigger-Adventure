using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Circle
{
    public class PlayerManager : MonoBehaviour
    {
        CheckpointManager checkman;
        private int health;

        private void Awake()
        {
            checkman = FindObjectOfType<CheckpointManager>();
            health = 3;
        }

        public void Hit()
        {
            health--;
            
            // Update UI here

            if (health > 0)
            {
                // set checkpoint, put gravity back to normal

            }
            else
            {
                // Death case, reset scene
            }
        }
    }
}
