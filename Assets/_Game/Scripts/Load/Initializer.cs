using AppsFlyerSDK;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] RectTransform handle;
    [SerializeField] TimeFetcher timeFetcher;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsInternetAvailable());
        DOVirtual.Float(0f, 1, 3f, value =>
        {
            SetProgress(value);
        });
        if (GameSettings.Instance.IsInternetTime)
        {
            timeFetcher.FetchTimeFromServer(1, (DateTime startupTime) =>
            {
                startupTime -= TimeSpan.FromSeconds(Time.realtimeSinceStartup);
                GameManager.Instance.SetStartupTime(startupTime);
            });
        }
        // Đợi để lấy time internet
        yield return new WaitForSeconds(1.5f);
        DataManager.Instance.LoadData();
        AudioManager.Instance.Initialize();
        AudioManager.Instance.PlayMusic(MusicId.Home);
        VibrationManager.Instance.Initialize();
        yield return null;
        MaxManager.Instance.Initialize();
        yield return null;
        FirebaseManager.Instance.Initialize();
        yield return null;
        AppsFlyerAdRevenue.start();
        yield return new WaitForSeconds(1.5f);
        SetProgress(1);
        yield return new WaitUntil(() => GameManager.Instance.IsInternetAvailable());
        LoadSceneManager.Instance.LoadScene(SceneId.Home, LoadSceneManager.Mode.Before);
    }

    void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        fillImage.fillAmount = progress;
        handle.anchoredPosition = new Vector2(progress * 490 - 245, 0);
    }
}
