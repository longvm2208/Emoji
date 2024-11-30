using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public static GameData Instance => DataManager.Instance.GameData;

    public bool showPopupRate;
    public int HintsCount;
    public int rateStar;
    [Header("LEVEL")]
    public int CurrentLevelIndex;
    public int SelectedLevelIndex;
    [Header("AUDIO & VIBRATION")]
    public bool IsAudioEnabled;
    public bool IsVibrationEnabled;

    public GameData()
    {
        showPopupRate = true;
        HintsCount = 0;
        rateStar = -1;
        // LEVEL
        CurrentLevelIndex = 0;
        SelectedLevelIndex = 0;
        // AUDIO & VIBRATION
        IsAudioEnabled = true;
        IsVibrationEnabled = true;
    }
}
