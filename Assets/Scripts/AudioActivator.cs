using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class AudioActivator : MonoBehaviour
    {
        private AudioSource source;
        public AudioSource Source => source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!source.isPlaying) gameObject.SetActive(false);
        }
    }
}
