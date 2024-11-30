using Google.Play.Review;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] bool isInternetAvailableEditor;
    [SerializeField] bool multiTouchEnabled = true;
    [SerializeField] int targetFps = 60;
    [SerializeField, Range(0.01f, 10)] float timeScale;
    [SerializeField] RectTransform canvasRt;
    public Vector2 CanvasSizeDelta => canvasRt.sizeDelta;
    public RectTransform CanvasRt => canvasRt;
    [SerializeField, ExposedScriptableObject]
    GameSettings gameSettings;
#if UNITY_ANDROID
    private ReviewManager reviewManager;
    private PlayReviewInfo playReviewInfor;
#endif
    public GameSettings GameSettings => gameSettings;
    public bool IsEnableAds => gameSettings.IsEnableAds;

    DateTime startupTime;
    public DateTime Now => startupTime + TimeSpan.FromSeconds(Time.realtimeSinceStartup);

    private float previousTimeScale;

    void Awake()
    {
        Application.targetFrameRate = targetFps;
        Input.multiTouchEnabled = multiTouchEnabled;

        startupTime = DateTime.Now;
        timeScale = Time.timeScale;
        previousTimeScale = timeScale;
    }

    private void Update()
    {
        if (previousTimeScale - timeScale != 0)
        {
            Time.timeScale = timeScale;
            previousTimeScale = timeScale;
        }

        if (UIManager.HasInstance)
        {
            if (IsInternetAvailable() && UIManager.Instance.IsPopupOpen(PopupId.NoInternet))
            {
                UIManager.Instance.Popup(PopupId.NoInternet).Close();
            }
            else if (!IsInternetAvailable() && !UIManager.Instance.IsPopupOpen(PopupId.NoInternet))
            {
                UIManager.Instance.OpenPopup(PopupId.NoInternet);
            }
        }
    }

    public void SetStartupTime(DateTime startupTime)
    {
        this.startupTime = startupTime;
    }

    #region INTERNET
    public bool IsInternetAvailable()
    {
#if UNITY_EDITOR
        return isInternetAvailableEditor;
#else
        return !(Application.internetReachability == NetworkReachability.NotReachable);
#endif
    }
    #endregion

    #region RATE GAME
    public void RateGameInApp()
    {
        StartCoroutine(RateGameInAppRoutine());
    }

    private IEnumerator RateGameInAppRoutine()
    {
        ////Debug.Log("Begin Rate Game In App Routine");
#if UNITY_ANDROID
        reviewManager = new ReviewManager();

        var requestFlowOperation = reviewManager.RequestReviewFlow();

        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            //Debug.LogError("Request flow operation error");
            yield break;
        }
        else
        {
            ////Debug.Log(requestFlowOperation.Error);
        }

        playReviewInfor = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfor);

        yield return launchFlowOperation;
        playReviewInfor = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            //Debug.LogError("Launch flow operation error");
            yield break;
        }
        else
        {
            //Debug.Log(launchFlowOperation.Error);
        }
        ////Debug.Log("End Rate Game In App Routine");
#elif UNITY_IOS
        yield return null;
        Device.RequestStoreReview();
#endif
    }
    #endregion
}
