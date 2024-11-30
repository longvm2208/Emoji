using System;
using UnityEngine;

public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
{
    public readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    public int LevelAmount;
    [Header("ADS")]
    public bool LoadingInterstitial = true;
    public float InterstitialCapping = 15;
    public float RewardedAdCapping = 15;
}
