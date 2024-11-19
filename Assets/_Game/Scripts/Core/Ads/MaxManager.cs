using AppsFlyerSDK;
using System;
using System.Collections;
using UnityEngine;

public class MaxManager : SingletonMonoBehaviour<MaxManager>
{
    const string Key = "ZoNyqu_piUmpl33-qkoIfRp6MTZGW9M5xk1mb1ZIWK6FN9EBu0TXSHeprC3LMPQI7S3kTc1-x7DJGSV8S-gvFJ";
    //const string UserId = "";

#if UNITY_ANDROID
    const string InterstitialAdUnitId = "a3222f2cfbfdcb59";
    const string RewardedAdUnitId = "4e205dc4973cecd1";
    const string BannerAdUnitId = "cd33e762b62de3b6";
    const string MrecAdUnitId = "4e16ac9dc828a1f2";
#elif UNITY_IOS
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MrecAdUnitId = "";
#else
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MrecAdUnitId = "";
#endif

    [SerializeField] bool isShowMediationDebugger;
    [SerializeField] bool isAdaptiveBanner;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject adNotLoadedYet;
    [SerializeField] Color bannerColor;

    bool isShowingInterstitial;
    bool isShowingRewardedAd;
    int interstitialRetryAttempt;
    int rewardedRetryAttempt;
    [SerializeField] float interstitialTimeCounter = -1;
    string place;

    public bool IsShowingAds => isShowingInterstitial || isShowingRewardedAd;
    public float BannerHeightInPixels { get; private set; }
    public float BannerHeight { get; private set; }

    private Action OnInterstitialAdDisplayed;
    private Action<MaxSdkBase.ErrorInfo> OnInterstitialAdFailedToDisplay;
    private Action OnInterstitialAdClicked;

    event Action rewardedAdReceivedReward;
    event Action rewardedAdDisplayed;
    event Action rewardedAdHidden;
    event Action<MaxSdkBase.ErrorInfo> rewardedAdFailedToDisplay;
    event Action rewardedAdClick;

    private void Update()
    {
        if (interstitialTimeCounter > 0)
        {
            interstitialTimeCounter -= Time.deltaTime;
        }
    }

    public void Initialize()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialized;
        MaxSdk.SetSdkKey(Key);
        //MaxSdk.SetUserId(UserId);
        MaxSdk.InitializeSdk();
        CalculateBannerHeight();
    }

    void OnInitialized(MaxSdkBase.SdkConfiguration configuration)
    {
        // AppLovin SDK is initialized, start loading ads
        InitializeInterstitialAds();
        InitializeRewardedAds();
        InitializeBannerAds();
        InitializeMRecAds();
        if (isShowMediationDebugger)
        {
            MaxSdk.ShowMediationDebugger();
        }
        StartCoroutine(ShowBannerRoutine());
    }

    IEnumerator ShowBannerRoutine()
    {
        yield return new WaitUntil(() => CanShowBanner());
        if (LoadSceneManager.Instance.CurrentScene != SceneId.Home)
        {
            ShowBanner();
        }
    }

    bool CanShowBanner() => DataManager.Instance.IsLoaded && LoadSceneManager.Instance.CurrentScene != SceneId.Load;

    public void LoadAndShowCmpFlow()
    {
        var cmpService = MaxSdk.CmpService;

        cmpService.ShowCmpForExistingUser(error =>
        {
            if (null == error)
            {
                // The CMP alert was shown successfully.
            }
        });
    }

    public bool IsPrivacyOptionRequired() => MaxSdk.GetSdkConfiguration().ConsentFlowUserGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr;

    private void AdNotLoadedYetNotify()
    {
        adNotLoadedYet.SetActive(false);
        adNotLoadedYet.SetActive(true);
    }

    #region AD REVENUE PAID
    void LogAdRevenuePaid(MaxSdkBase.AdInfo adInfo)
    {
        var data = new ImpressionData(
            "appLovin",
            adInfo.AdFormat,
            MaxSdk.GetSdkConfiguration().CountryCode,
            adInfo.AdUnitIdentifier,
            adInfo.NetworkName,
            adInfo.Placement,
            adInfo.Revenue);

        FirebaseManager.Instance.LogAdRevenuePaid(data);

        AppsFlyerAdRevenue.logAdRevenue("applovin_max",
            AppsFlyerAdRevenueMediationNetworkType.
            AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
            data.revenue, "USD", data.ToDictionary());
    }
    #endregion

    #region INTERSTITIAL ADS
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

        // Load the first interstitial
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        interstitialRetryAttempt = 0;
        AppsflyerEventRegister.af_interstitial_ad_api_called();
        FirebaseManager.Instance.ad_inter_load();
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("displayed");
        AppsflyerEventRegister.af_interstitial_ad_displayed();
        blocker.SetActive(false);
        OnInterstitialAdDisplayed?.Invoke();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("display failed");
        isShowingInterstitial = false;
        LoadInterstitial();
        blocker.SetActive(false);
        OnInterstitialAdFailedToDisplay?.Invoke(errorInfo);
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnInterstitialAdClicked?.Invoke();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("hidden");
        isShowingInterstitial = false;
        blocker.SetActive(false);
        LoadInterstitial();
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial revenue paid");
        LogAdRevenuePaid(adInfo);
    }

    public void ShowInterstitial(string placement, Action onFinished = null)
    {
        if (!GameManager.Instance.IsEnableAds || interstitialTimeCounter > 0)
        {
            onFinished?.Invoke();
            return;
        }

        Debug.Log("Show interstitial ad");

        AppsflyerEventRegister.af_interstitial_ad_eligible();
        blocker.SetActive(true);

        OnInterstitialAdDisplayed = () =>
        {
            FirebaseManager.Instance.ad_inter_show();
            onFinished?.Invoke();
        };

        OnInterstitialAdFailedToDisplay = (errorInfo) =>
        {
            FirebaseManager.Instance.ad_inter_fail(errorInfo.Message);
            onFinished?.Invoke();
        };

        OnInterstitialAdClicked = () =>
        {
            FirebaseManager.Instance.ad_inter_click();
        };

        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        {
            isShowingInterstitial = true;
            interstitialTimeCounter = ConfigManager.Instance.InterstitialCapping;
            MaxSdk.ShowInterstitial(InterstitialAdUnitId);
        }
        else
        {
            Debug.Log("Interstitial ad not ready");
            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
            blocker.SetActive(false);
            onFinished?.Invoke();
        }
    }
    #endregion

    #region REWARDED AD
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        // Load the first rewarded ad
        LoadRewardedAd();
    }

    void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        rewardedRetryAttempt = 0;
        AppsflyerEventRegister.af_rewarded_ad_api_called();
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppsflyerEventRegister.af_rewarded_ad_displayed();
        rewardedAdDisplayed?.Invoke();
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        isShowingRewardedAd = false;
        LoadRewardedAd();
        blocker.SetActive(false);
        rewardedAdFailedToDisplay?.Invoke(errorInfo);
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        isShowingRewardedAd = false;
        blocker.SetActive(false);
        rewardedAdHidden?.Invoke();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        AppsflyerEventRegister.af_rewarded_ad_completed();
        isShowingRewardedAd = false;
        blocker.SetActive(false);
        rewardedAdReceivedReward?.Invoke();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LogAdRevenuePaid(adInfo);
    }

    public void ShowRewardedAd(string placement, Action onSuccess = null, Action onFail = null)
    {
        if (!GameManager.Instance.IsEnableAds)
        {
            onSuccess?.Invoke();
            return;
        }
        rewardedAdDisplayed = () =>
        {
            FirebaseManager.Instance.ads_reward_show(placement);
        };
        rewardedAdReceivedReward = () =>
        {
            FirebaseManager.Instance.ads_reward_complete(placement);
            onSuccess?.Invoke();
        };

        rewardedAdFailedToDisplay = (errorInfo) =>
        {
            FirebaseManager.Instance.ads_reward_fail(placement, errorInfo.Message);
            onFail?.Invoke();
        };
        AppsflyerEventRegister.af_rewarded_ad_eligible();
        this.place = placement;
        blocker.SetActive(true);
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            isShowingRewardedAd = true;
            interstitialTimeCounter = ConfigManager.Instance.InterstitialCapping;
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready");
            LoadRewardedAd();
            blocker.SetActive(false);
            AdNotLoadedYetNotify();
            onFail?.Invoke();
        }
    }
    #endregion

    #region BANNER AD
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320�50 on phones and 728�90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        if (isAdaptiveBanner)
        {
            MaxSdk.SetBannerExtraParameter(BannerAdUnitId, "adaptive_banner", "true");
        }

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, bannerColor);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }

    void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        LogAdRevenuePaid(adInfo);
    }

    void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowBanner()
    {
        if (!GameManager.Instance.IsEnableAds) return;
        MaxSdk.ShowBanner(BannerAdUnitId);
    }

    public void HideBanner()
    {
        if (!GameManager.Instance.IsEnableAds) return;
        MaxSdk.HideBanner(BannerAdUnitId);
    }

    void CalculateBannerHeight()
    {
        BannerHeight = 50;
        if (isAdaptiveBanner)
        {
            BannerHeight = MaxSdkUtils.GetAdaptiveBannerHeight();
        }
        else if (MaxSdkUtils.IsTablet())
        {
            BannerHeight = 90f;
        }
        BannerHeight *= MaxSdkUtils.GetScreenDensity();
        BannerHeightInPixels = BannerHeight;
        BannerHeight /= canvas.scaleFactor;
    }
    #endregion

    #region MREC AD
    public void InitializeMRecAds()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(MrecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;
    }

    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error) { }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LogAdRevenuePaid(adInfo);
    }

    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowMRec(int x, int y)
    {
        if (!GameManager.Instance.IsEnableAds) return;
        MaxSdk.UpdateMRecPosition(MrecAdUnitId, x, y);
        MaxSdk.ShowMRec(MrecAdUnitId);
        HideBanner();
    }

    public void ShowMRec(RectTransform rt, AnchorsType anchorsType = AnchorsType.Center)
    {
        if (!GameManager.Instance.IsEnableAds) return;
        Vector2 canvasSizeDelta = GameManager.Instance.CanvasRt.sizeDelta;
        Vector2 pos = rt.anchoredPosition;
        switch (anchorsType)
        {
            case AnchorsType.Top:
                pos.y += canvasSizeDelta.y;
                break;
            case AnchorsType.Bottom:
                pos.y -= canvasSizeDelta.y * 0.5f;
                break;
        }
        pos = new Vector2(pos.x + canvasSizeDelta.x * 0.5f, canvasSizeDelta.y * 0.5f - pos.y);
        pos *= canvas.scaleFactor;
        pos /= MaxSdkUtils.GetScreenDensity();
        pos -= 0.5f * new Vector2(300, 250);
        MaxSdk.UpdateMRecPosition(MrecAdUnitId, pos.x, pos.y);
        MaxSdk.ShowMRec(MrecAdUnitId);
        HideBanner();
    }

    public void ShowMRec(MaxSdkBase.AdViewPosition position)
    {
        if (!GameManager.Instance.IsEnableAds) return;
        MaxSdk.UpdateMRecPosition(MrecAdUnitId, position);
        MaxSdk.ShowMRec(MrecAdUnitId);
        HideBanner();
    }

    public void HideMRec()
    {
        if (!GameManager.Instance.IsEnableAds) return;
        MaxSdk.HideMRec(MrecAdUnitId);
        ShowBanner();
    }
    #endregion
}

public enum AnchorsType
{
    Top,
    Center,
    Bottom,
}
