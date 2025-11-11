using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Utilities.Core;

namespace Circle
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;

        private void Start()
        {
            if (videoPlayer != null)
            {
                videoPlayer.loopPointReached += OnVideoEnd;
                videoPlayer.prepareCompleted += OnVideoPrepared;
                videoPlayer.Prepare();
            }
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            vp.Play();
        }

        private void OnVideoEnd(VideoPlayer vp)
        {
            // Load the next scene or perform any action after the video ends
            vp.Stop();
            vp.targetTexture.Release();

            StartCoroutine(SceneTools.TransitionToNextScene());
        }
    }
}
