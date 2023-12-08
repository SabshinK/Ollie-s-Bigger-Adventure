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
            /*
             * I'm checking the clip here because I'm trying to avoid potential cases where the audio
             * object is enabled, then update is called but it's still not playing the clip yet, and
             * so the object deactivates itself before it even plays anything (basically a data race),
             * and the clip is set before any of this happens. Basically the clip is a latch
             */
            if (!source.isPlaying && source.clip != null)
            {
                source.clip = null;
                gameObject.SetActive(false);
            }
        }
    }
}
