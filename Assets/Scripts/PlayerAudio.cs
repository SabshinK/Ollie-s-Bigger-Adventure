using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

namespace Circle
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioTrack footstepTrack;
        [SerializeField] private AudioTrack chargeGravityTrack;
        [SerializeField] private AudioTrack gravityUp;
        [SerializeField] private AudioTrack gravityDown;

        [Space]
        [Header("References")]
        [SerializeField] private AnimationEventHandler footstepEvent;

        [SerializeField] private AudioSource gravitySource;

        private AudioPool audioPool;

        private InputAction gravityAction;

        /*
         * I probably could get the gravity direction from the character controller or something,
         * but I worry that this value isn't going to be correct between function calls, so I'm
         * just keeping track of it here too.
         */
        private int gravityDirection;

        private void Awake()
        {
            audioPool = FindObjectOfType<AudioPool>();
            gravityDirection = (int)Vector3.Normalize(Physics.gravity).y;
            gravityAction = InputHandler.GetAction("Toggle Gravity");
        }

        private void OnEnable()
        {
            gravityAction.started += PlayChargeGravity;
            gravityAction.performed += PlayGravityChanged;
            gravityAction.canceled += CancelChargeGravity;

            footstepEvent.animEvent.AddListener(PlayFootsteps);
        }

        private void OnDisable()
        {
            gravityAction.started -= PlayChargeGravity;
            gravityAction.performed -= PlayGravityChanged;
            gravityAction.canceled -= CancelChargeGravity;

            footstepEvent.animEvent.RemoveListener(PlayFootsteps);
        }

        private void PlayFootsteps()
        {
            int index = Random.Range(0, footstepTrack.clips.Length);
            float pitch = Random.Range(0.75f, 1.25f);

            audioPool.PlayClipAtPoint(footstepTrack.clips[index], transform.position, footstepTrack.volume, pitch);
        }

        #region Charge Gravity Methods

        private void PlayChargeGravity(InputAction.CallbackContext context)
        {
            var interaction = context.interaction as HoldInteraction;
            StartCoroutine(ChargeGravity(interaction.duration));
        }

        private IEnumerator ChargeGravity(float holdDuration)
        {
            float timer = 0;
            GameObject audioObj = audioPool.GetPooledObject();
            gravitySource = audioObj.GetComponent<AudioSource>();

            gravitySource.clip = chargeGravityTrack.clips[0];
            gravitySource.pitch = 1f;
            gravitySource.volume = chargeGravityTrack.volume;
            gravitySource.loop = true;

            audioObj.SetActive(true);
            gravitySource.Play();

            while (timer < holdDuration)
            {
                // Update the timer
                timer += Time.deltaTime;

                gravitySource.pitch += timer / holdDuration;

                yield return new WaitForEndOfFrame();
            }

            gravitySource.pitch = 2f;
            gravitySource.loop = false;
            gravitySource.Stop();
        }

        private void CancelChargeGravity(InputAction.CallbackContext context)
        {
            StopAllCoroutines();
            gravitySource.loop = false;
            gravitySource.Stop();
        }

        #endregion

        private void PlayGravityChanged(InputAction.CallbackContext context)
        {
            AudioTrack track = gravityDirection < 0 ? gravityUp : gravityDown;

            audioPool.PlayClipAtPoint(track.clips[0], transform.position, track.volume);

            gravityDirection *= -1;
        }
    }

    [System.Serializable]
    struct AudioTrack
    {
        public AudioClip[] clips;

        public float volume;
    }
}
