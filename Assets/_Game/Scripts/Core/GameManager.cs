using System;
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
}
