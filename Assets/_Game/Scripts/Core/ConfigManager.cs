using System;
using UnityEngine;

public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
{
    public readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    public int LevelAmount;
    [Header("ADS")]
    public float InterstitialAdCapping = 40f;
    public float RewardedAdCapping = 40f;
}
