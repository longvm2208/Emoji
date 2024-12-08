using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour 
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject canvasGo;
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

        imageRt.ChangeAnchorPosY(canvasSizeDelta.y * (0.5f - 0.33f));

        canvasGo.SetActive(false);
        imageRt.gameObject.SetActive(false);
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
    }

    private void OnLoopPointReached(VideoPlayer source)
    {
        Debug.Log("End video");
        onComplete?.Invoke();
        //imageRt.gameObject.SetActive(false);
    }

    IEnumerator CheckPrepareCoroutine()
    {
        yield return new WaitUntil(() => videoPlayer.isPrepared);
    }

    public void Play()
    {
        Debug.Log("Play video");
        videoPlayer.Play();
        canvasGo.SetActive(true);
        imageRt.gameObject.SetActive(true);
    }
}
