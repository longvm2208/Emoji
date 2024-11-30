using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Document link : https://docs.google.com/spreadsheets/d/1PUjPCuHoE5pRhD8Up4vCrQWRktgS_MFFADkgZppNdEw/edit#gid=0
/// </summary>
public class FirebaseManager : SingletonMonoBehaviour<FirebaseManager>
{
    private bool ready;

    private Queue<CachedFirebaseEvent> cachedFirebaseEvents = new();

    #region INITIALIZE
    public void Initialize()
    {
        Debug.Log("Initialize firebase");

        StartCoroutine(LogCachedEventsRoutine());

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                ready = true;

                //Crashlytics.ReportUncaughtExceptionsAsFatal = true;

                FetchDataAsync();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private IEnumerator LogCachedEventsRoutine()
    {
        yield return new WaitUntil(() => ready);

        if (cachedFirebaseEvents.Count > 0)
        {
            for (int i = 0; i < cachedFirebaseEvents.Count; i++)
            {
                LogCachedEvent(cachedFirebaseEvents.Dequeue());
            }
        }
    }

    private void LogCachedEvent(CachedFirebaseEvent cachedEvent)
    {
        LogEvent(cachedEvent.name, cachedEvent.parameters);
    }
    #endregion

    #region SET USER PROPERTIES
    private void SetUserProperty(string property, string value)
    {
        if (Debug.isDebugBuild || Application.isEditor || !ready) return;
        FirebaseAnalytics.SetUserProperty(property, value);
    }
    #endregion

    #region REMOTE CONFIG
    public Task FetchDataAsync()
    {
        Debug.Log("Fetch remote config data");

        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            ApplyRemoteConfigData();
        });
    }

    private int GetInt(string key)
    {
        return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
    }

    private bool GetBool(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    }

    private void ApplyRemoteConfigData()
    {
        Debug.Log("Apply remote config data");
        ConfigManager.Instance.InterstitialCapping = GetInt("interstitial_capping");
        ConfigManager.Instance.LoadingInterstitial = GetBool("loading_interstitial");
    }
    #endregion

    #region LOG EVENT
    public void LogEvent(string name, params Parameter[] parameters)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        if (!ready)
        {
            cachedFirebaseEvents.Enqueue(new CachedFirebaseEvent(name, parameters));
        }
        else
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }

    public void LogEvent(string name)
    {
        if (Debug.isDebugBuild || Application.isEditor || !ready) return;
        FirebaseAnalytics.LogEvent(name);
    }
    #endregion

    #region AD REVENUE PAID
    public void LogAdRevenuePaid(ImpressionData data)
    {
        Parameter[] parameters = data.ToParameters();
        LogEvent("ad_impression", parameters);
    }
    #endregion

    #region ADS
    public void ads_reward_offer(string placement)
    {
        LogEvent(nameof(ads_reward_offer),
            new Parameter(nameof(placement), placement));
    }

    public void ads_reward_click(string placement)
    {
        LogEvent(nameof(ads_reward_click),
            new Parameter(nameof(placement), placement));
    }

    public void ads_reward_show(string placement)
    {
        LogEvent(nameof(ads_reward_show),
            new Parameter(nameof(placement), placement));
    }

    public void ads_reward_fail(string placement, string error)
    {
        LogEvent(nameof(ads_reward_fail),
            new Parameter(nameof(placement), placement),
            new Parameter(nameof(error), error));
    }

    public void ads_reward_complete(string placement)
    {
        LogEvent(nameof(ads_reward_complete),
            new Parameter(nameof(placement), placement));
    }

    public void ad_inter_fail(string error)
    {
        LogEvent(nameof(ad_inter_fail), new Parameter(nameof(error), error));
    }

    public void ad_inter_load()
    {
        LogEvent(nameof(ad_inter_load));
    }

    public void ad_inter_show()
    {
        LogEvent(nameof(ad_inter_show));
    }

    public void ad_inter_click()
    {
        LogEvent(nameof(ad_inter_click));
    }
    #endregion

    #region LEVEL
    public void level_start(int level)
    {
        LogEvent(nameof(level_start), new Parameter(nameof(level), (level + 1).ToString()));
    }

    public void level_complete(int level, int timeplayed)
    {
        LogEvent(nameof(level_complete),
            new Parameter(nameof(level), (level + 1).ToString()),
            new Parameter(nameof(timeplayed), timeplayed.ToString()));
    }

    public void level_fail(int level, int failcount)
    {
        LogEvent(nameof(level_fail),
            new Parameter(nameof(level), (level + 1).ToString()),
            new Parameter(nameof(failcount), failcount.ToString()));
    }

    public void level_checkpoint(int level)
    {
        LogEvent("checkpoint_" + level);
        AppsflyerEventRegister.af_level_achieved(level);
    }
    #endregion
}
