using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using UnityEngine;

public class AdMobManager : SingletonMonoBehaviour<AdMobManager>
{
#if UNITY_ANDROID
    // TEST ID
    //private string appOpenAdUnitId = "ca-app-pub-3940256099942544/9257395921";
    //private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    // RELEASE ID
    private string appOpenAdUnitId = "ca-app-pub-9819920607806935/4603691114";
    const string InterstitialAdUnitId = "ca-app-pub-9819920607806935/7448540407";
    //private string bannerAdUnitId = "ca-app-pub-9819920607806935/9603571357";
#elif UNITY_IOS
    private string appOpenAdUnitId = "";
    private string bannerAdUnitId = "";
#else
    private string appOpenAdUnitId = "";
    private string bannerAdUnitId = "";
#endif

    private AppOpenAd appOpenAd;
    private DateTime expireTime;
    private BannerView bannerView;
    private BannerView collapsibleBannerView;
    private InterstitialAd interstitialAd;
    Action onInterstitialFinish;

    private bool isInitialized = false;

    public bool IsAppOpenAdAvailable
    {
        get
        {
            return appOpenAd != null && appOpenAd.CanShowAd() && DateTime.Now < expireTime;
        }
    }

    private void Awake()
    {
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log($"App state changed to: {state}");

        if (state == AppState.Foreground)
        {
            //if (IsAppOpenAdAvailable && ConfigManager.Instance.IsAOAOnSwitch)
            //{
                ShowAppOpenAd();
            //}
        }
    }

    public void Initialize()
    {
        Debug.Log("Initialize AdMob");

        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        MobileAds.Initialize(status =>
        {
            Debug.Log("AdMob initialized successfully");

            isInitialized = true;

            LoadAppOpenAd();
            LoadInterstitialAd();
            //LoadBannerAd(false);
        });

        ShowAOAOnOpen();
    }

    void ShowAOAOnOpen()
    {
        StartCoroutine(ShowAOAOnOpenRoutine());
    }

    IEnumerator ShowAOAOnOpenRoutine()
    {
        yield return new WaitUntil(() => IsAppOpenAdAvailable);
        if (LoadSceneManager.Instance.CurrentScene == SceneId.Load)
        {
            yield return new WaitUntil(() => LoadSceneManager.Instance.CurrentScene != SceneId.Load);
            ShowAppOpenAd();
        }
    }

    #region APP OPEN AD
    public void LoadAppOpenAd()
    {
        Debug.Log("Load app open ad");

        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        var adRequest = new AdRequest();

        AppOpenAd.Load(appOpenAdUnitId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("App open ad failed to load an ad with error: " + error);
            }
            else
            {
                Debug.Log("App open ad loaded with response: " + ad.GetResponseInfo());

                expireTime = DateTime.Now + TimeSpan.FromHours(4);
                appOpenAd = ad;
                RegisterEventHandlers(ad);
            }
        });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.LogFormat("App open ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content with error: " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }

    public void ShowAppOpenAd()
    {
        if (!GameManager.Instance.IsEnableAds || MaxManager.Instance.IsShowingAds) return;

        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Show app open ad");

            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }
    #endregion

    #region INTERSTITIAL
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(InterstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public bool CanShowInterstitial()
    {
        return interstitialAd != null && interstitialAd.CanShowAd();
    }

    public void ShowInterstitialAd(Action onFinish = null)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            onInterstitialFinish = onFinish;
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            onFinish?.Invoke();
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
            FirebaseManager.Instance.ad_inter_click();
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            FirebaseManager.Instance.ad_inter_show();
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            onInterstitialFinish?.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            onInterstitialFinish?.Invoke();
            FirebaseManager.Instance.ad_inter_fail(error.GetMessage());
        };
    }
    #endregion
}
