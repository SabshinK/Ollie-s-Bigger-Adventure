using UnityEngine;

public class SirenTrigger1 : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public AudioSource audioSource; // Reference to the AudioSource component
    public string animationName; // The name of the animation to play
    public static float audioSpeedMultiplier = 1.0f; // Static to keep the speed increase across triggers

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
                audioSpeedMultiplier *= 1.25f;
                audioSource.pitch = audioSpeedMultiplier;
            }
        }
    }
}
