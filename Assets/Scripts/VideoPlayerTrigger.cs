using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Circle
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerTrigger : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = 0;
        [SerializeField] private string videoFileName;

        [SerializeField] private GameObject videoImg;
        private VideoPlayer vp;

        private void Awake()
        {
            vp = GetComponent<VideoPlayer>();
            vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            vp.Play();
        }

        private void OnEnable()
        {
            vp.loopPointReached += EndVideo;
        }

        private void OnDisable()
        {
            vp.loopPointReached -= EndVideo;
        }

        private void EndVideo(VideoPlayer vp)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
