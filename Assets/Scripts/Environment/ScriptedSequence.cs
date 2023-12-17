using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Circle
{
    public class ScriptedSequence : MonoBehaviour
    {
        [Tooltip("The virtual camera that follows the player.")]
        [SerializeField] private CinemachineVirtualCamera playerVCam;

        [Tooltip("The virtual camera to cut to for the goal.")]
        [SerializeField] private CinemachineVirtualCamera goalVCam;

        private Animator anim;
        private CinemachineBrain brain;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                StartCoroutine(Trapped());
        }

        private IEnumerator Trapped()
        {
            // Do cutscene stuff
            GameManager.ToggleCutscene();

            yield return new WaitForSeconds(0.5f);

            anim.SetTrigger("Close");

            yield return new WaitForSeconds(0.5f);

            playerVCam.Priority = 0;
            goalVCam.Priority = 1;

            yield return new WaitForSeconds(3f);

            playerVCam.Priority = 1;
            goalVCam.Priority = 0;

            yield return new WaitForSeconds(1f);

            GameManager.ToggleCutscene();

            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
