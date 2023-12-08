using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Circle
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioTrack footstepTrack;
        [SerializeField] private AudioTrack chargeGravity;
        [SerializeField] private AudioTrack gravityUp;
        [SerializeField] private AudioTrack gravityDown;

        [Space]
        [Header("References")]
        [SerializeField] private AnimationEventHandler footstepEvent;

        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioSource gravitySource;

        private AudioPool audioPool;

        private void Awake()
        {
            audioPool = FindObjectOfType<AudioPool>();
        }

        private void OnEnable()
        {
            footstepEvent.animEvent.AddListener(PlayFootsteps);
        }

        private void OnDisable()
        {
            footstepEvent.animEvent.RemoveListener(PlayFootsteps);
        }

        private void PlayFootsteps()
        {
            int index = Random.Range(0, footstepTrack.clips.Length);
            float pitch = Random.Range(0.75f, 1.25f);

            audioPool.PlayClipAtPoint(footstepTrack.clips[index], transform.position, pitch, footstepTrack.volume);
        }

        private void PlayChargeGravity()
        {

        }

        private void UpdateChargeGravity(float pitch)
        {

        }

        private void PlayGravityChanged()
        {

        }
    }

    [System.Serializable]
    struct AudioTrack
    {
        public AudioClip[] clips;

        public float volume;
    }
}
