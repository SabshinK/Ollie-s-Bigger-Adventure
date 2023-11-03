using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Circle
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera targetVCam;
        [SerializeField] private CinemachineVirtualCamera playerVCam;

        private void OnTriggerEnter(Collider other)
        {
            targetVCam.Priority = 1;
            playerVCam.Priority = 0;            
        }

        private void OnTriggerExit(Collider other)
        {
            playerVCam.Priority = 1;
            targetVCam.Priority = 0;
        }
    }
}
