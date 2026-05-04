using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public RenderTexture renderTexture;

    void Start()
    {
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        source.playbackSpeed = 0.75f;   
        source.Play();
    }
}