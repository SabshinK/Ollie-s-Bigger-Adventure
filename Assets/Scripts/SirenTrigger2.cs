using UnityEngine;

namespace Circle
{
    public class SirenTrigger2 : MonoBehaviour
    {
        public Animator animator;
        public AudioSource audioSource; // Original audio source
        public AudioSource AIaudio; // Additional audio source for AI audio
        public AudioSource noiseAudio; // Additional audio source for noise
        public AudioSource AIaudio2; // Second AI audio source to replace AIaudio on the fourth trigger
        public string animationName;

        private PlayerManager player;
        private BoxCollider trigger;
        private static int triggerCount = 0; // Track the number of times the trigger has been activated

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
            // Ensure all audios start with their intended pitch and are not playing
            SetupAudioSource(audioSource);
            SetupAudioSource(AIaudio);
            SetupAudioSource(noiseAudio);
            SetupAudioSource(AIaudio2);

            animator.speed = 0;
        }

        private void SetupAudioSource(AudioSource source)
        {
            if (source != null)
            {
                source.pitch = 1.0f;
                source.Stop(); // Ensure the audio source is not playing
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Ensure only the player can trigger the effects
            {
                animator.Play(animationName, -1, 0f);

                triggerCount++; // Increment trigger count

                // Decide which audio to play based on the trigger count
                switch (triggerCount)
                {
                    case 1:
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                        break;
                    case 2:
                        if (!AIaudio.isPlaying)
                        {
                            AIaudio.Play();
                        }
                        break;
                    case 3:
                        if (!noiseAudio.isPlaying)
                        {
                            noiseAudio.Play();
                        }
                        break;
                    case 4:
                        if (!AIaudio2.isPlaying)
                        {
                            AIaudio.Stop();
                            AIaudio2.Play();
                        }
                        break;
                }

                trigger.enabled = false; // Turn off trigger to prevent re-entry before reset
            }
        }

        private void Reset()
        {
            trigger.enabled = true;
            triggerCount = 0; // Reset trigger count

            // Stop all audio sources to ensure a clean state
            audioSource.Stop();
            AIaudio.Stop();
            noiseAudio.Stop();
            AIaudio2.Stop();

            animator.Play("SirenLight 0", -1, 0f);
        }
    }
}
