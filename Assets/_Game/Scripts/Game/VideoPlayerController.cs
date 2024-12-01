using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour 
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RectTransform imageRt;
    [SerializeField] UnityEvent onComplete;

    private void OnValidate()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer.targetCamera == null) videoPlayer.targetCamera = Camera.main;
    }

    private void Start()
    {
        videoPlayer.Prepare();
        StartCoroutine(CheckPrepareCoroutine());
        Vector2 canvasSizeDelta = GameManager.Instance.CanvasSizeDelta;
        float baseRatio = 1080 / 1920f;
        float currentRatio = canvasSizeDelta.x / canvasSizeDelta.y;
        if (baseRatio > currentRatio)
        {
            imageRt.sizeDelta *= baseRatio / currentRatio;
        }
        else
        {
            imageRt.sizeDelta *= currentRatio / baseRatio;
        }

        imageRt.gameObject.SetActive(false);
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
    }

    private void OnLoopPointReached(VideoPlayer source)
    {
        onComplete?.Invoke();
        //imageRt.gameObject.SetActive(false);
    }

    IEnumerator CheckPrepareCoroutine()
    {
        yield return new WaitUntil(() => videoPlayer.isPrepared);
    }

    public void Play()
    {
        videoPlayer.Play();
        imageRt.gameObject.SetActive(true);
    }
}
