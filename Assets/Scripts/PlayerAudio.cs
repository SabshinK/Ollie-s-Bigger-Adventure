using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Circle
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] footstepClips;
        [SerializeField] private AudioClip chargeGravity;
        [SerializeField] private AudioClip gravityUp;
        [SerializeField] private AudioClip gravityDown;

        [Space]
        [Header("References")]
        [SerializeField] private AnimationEventHandler footstepEvent;

        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioSource gravitySource;

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
            int index = Random.Range(0, footstepClips.Length);
            float pitch = Random.Range(0.75f, 1.25f);

            footstepSource.clip = footstepClips[index];
            footstepSource.pitch = pitch;

            
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
}
