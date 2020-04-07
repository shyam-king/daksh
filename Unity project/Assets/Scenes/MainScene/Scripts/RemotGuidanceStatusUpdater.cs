using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RemotGuidanceStatusUpdater : MonoBehaviour
{
    public VideoPlayer mVideoPlayer;
    public VideoClip[] videoSources = new VideoClip[4];
    public RawImage mImage;

    public Button mNextButton;
    public Button mPrevButton;

    bool playing = true;
    int currentSource = 0;

    // Start is called before the first frame update
    void Start()
    {
        mVideoPlayer.clip = videoSources[currentSource];
        mVideoPlayer.source = VideoSource.VideoClip;
        mVideoPlayer.Play();

        if (mNextButton != null)
        {
            mNextButton.onClick.AddListener(OnNextButtonClick);
        }

        if (mPrevButton != null)
        {
            mPrevButton.onClick.AddListener(OnPrevButtonClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mVideoPlayer.clip = videoSources[currentSource];
        mImage.texture = mVideoPlayer.texture;

        mPrevButton.gameObject.SetActive(currentSource != 0);
        mNextButton.gameObject.SetActive(currentSource != videoSources.Length - 1);
    }

    void OnNextButtonClick()
    {
        currentSource++;
        if (currentSource >= videoSources.Length)
            currentSource = videoSources.Length - 1;
    }

    void OnPrevButtonClick()
    {
        currentSource--;
        if (currentSource < 0)
            currentSource = 0;
    }
}
