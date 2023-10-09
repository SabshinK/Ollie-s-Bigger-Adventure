using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Circle
{
    public class PlayerManager : MonoBehaviour
    {
        private CheckpointManager checkman;
        private ReverseGravityAbility rgAbility;

        private Rigidbody rb;

        public delegate void OnHit();
        public event OnHit onHit;

        [SerializeField] private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        [SerializeField] private int defaultHealth = 3;
        public int DefaultHealth => defaultHealth;

        private void Awake()
        {
            // Do some searches to find these components, in case they aren't on the top level object
            checkman = GetComponentInChildren<CheckpointManager>();
            rgAbility = GetComponentInChildren<ReverseGravityAbility>();

            rb = GetComponentInChildren<Rigidbody>();

            health = defaultHealth;
        }

        private void OnEnable()
        {
            onHit += HitResponse;

            health = defaultHealth;
        }

        private void OnDisable()
        {
            onHit -= HitResponse;
        }

        public void RegisterHit()
        {
            health--;
            onHit?.Invoke();
        }

        private void HitResponse()
        {
            if (health > 0)
            {
                // set checkpoint, put gravity back to normal
                transform.position = checkman.Current.position;                
                rgAbility.ResetGravity();
            }
            else
            {
                // Death case
                StartCoroutine(Death());
            }
        }

        private IEnumerator Death()
        {
            // Game over splash screen

            yield return new WaitForSeconds(1);

            // Reload scene
            SceneManager.LoadScene(0);
        }
    }
}
