using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class webglVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject playButtonContainerObj;
    public GameObject pauseButtonObj;

    Ray ray;
    Camera mainCamera;
    RaycastHit hit;

    public static webglVideoPlayer instance;

    bool isVideoPlayedOnce = false;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;    
    }

    public void OnClickPlayButton()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.url = "https://parth-bucket-hlis.s3.eu-west-1.amazonaws.com/videoplayback.mp4";//System.IO.Path.Combine(Application.streamingAssetsPath, "videotenis.mp4");
        videoPlayer.Prepare();
        playButtonContainerObj.transform.GetChild(0).gameObject.SetActive(false);
        while(!videoPlayer.isPrepared)
        {
            yield return null;
        }
        playButtonContainerObj.SetActive(false);
        videoPlayer.Play();
        videoPlayer.isLooping = true;
        isVideoPlayedOnce = true;
        pauseButtonObj.SetActive(true);
        playButtonContainerObj.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.name == "VideoPlayButton" || hit.transform.name == "VideoPlayPauseButton")
                {
                    if (!isVideoPlayedOnce)
                        OnClickPlayButton();
                    else
                        pausePlayVideo();
                }
                else if(hit.transform.name == "VideoPauseButton")
                {
                    pausePlayVideo();
                }
                //else if(hit.transform.name == "VideoPlayPauseButton")
                //{
                //    pausePlayVideo();
                //}

            }
        }
    }

    public void pausePlayVideo()
    {
        if(videoPlayer.isPaused)
        {
            videoPlayer.Play();
            pauseButtonObj.SetActive(true);
            playButtonContainerObj.SetActive(false);
        }
        else
        {
            videoPlayer.Pause();
            pauseButtonObj.SetActive(false);
            playButtonContainerObj.transform.GetChild(0).gameObject.SetActive(true);
            playButtonContainerObj.SetActive(true);
        }
    }

    public void muteVideo()
    {
        if(videoPlayer.GetDirectAudioMute(0))
        {
            videoPlayer.SetDirectAudioMute(0, false);
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }
    }
}
