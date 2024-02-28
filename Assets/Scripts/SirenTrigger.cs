using UnityEngine;

namespace Circle 
{
    public class SirenTrigger : MonoBehaviour
    {
        public Animator animator; // Reference to the Animator component
        public AudioSource audioSource; // Reference to the AudioSource component
        public string animationName; // The name of the animation to play
        public static float audioSpeedMultiplier = 1.0f; // Static to keep the speed increase across triggers

        private PlayerManager player;
        private BoxCollider trigger;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            trigger = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            player.onHit += Reset;
        }

        private void OnDisable()
        {
            player.onHit -= Reset;
        }

        private void Start()
        {
            // Ensure the audioSource starts with the correct pitch
            audioSource.pitch = audioSpeedMultiplier;
            animator.speed = 0; // Stops all animations
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Ensure only the player can trigger the effects
            {
                // Play specified animation
                animator.Play(animationName, -1, 0f);

                // Play audio only if it's not already playing or if it's the first trigger
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                else
                {
                    // Increase audio playback speed after each trigger
                    audioSpeedMultiplier *= 1.0f;
                    audioSource.pitch = audioSpeedMultiplier;
                }

                // Turn off trigger
                trigger.enabled = false;
            }
        }

        private void Reset()
        {
            trigger.enabled = true;
            audioSpeedMultiplier = 1f;
            audioSource.pitch = 1f;
            audioSource.Stop();

            // Reset Animator
            animator.Play("SirenLight 0", -1, 0f);
            //animator.StartPlayback();
            //animator.StopPlayback();
            //animator.speed = 1;
        }
    }
}
