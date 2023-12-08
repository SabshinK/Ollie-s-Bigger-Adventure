using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circle
{
    public class AudioPool : ObjectPool
    {
        public void PlayClipAtPoint(AudioClip clip, Vector3 position, float pitch = 1f, float volume = 1f)
        {
            GameObject audioObj = GetPooledObject();
            AudioActivator activator = audioObj.GetComponent<AudioActivator>();
            AudioSource source = activator.Source;

            source.clip = clip;
            audioObj.transform.position = position;
            source.pitch = pitch;
            source.volume = volume;

            audioObj.SetActive(true);
            source.Play();
        }
    }
}
