using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public static GameData Instance => DataManager.Instance.GameData;

    [Header("LEVEL")]
    public int CurrentLevelIndex;
    public int SelectedLevelIndex;
    [Header("AUDIO & VIBRATION")]
    public bool IsAudioEnabled;
    public bool IsVibrationEnabled;

    public GameData()
    {
        // LEVEL
        CurrentLevelIndex = 0;
        SelectedLevelIndex = 0;
        // AUDIO & VIBRATION
        IsAudioEnabled = true;
        IsVibrationEnabled = true;
    }
}
