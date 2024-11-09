using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : ScriptableObject
{
    public static GameSettings Instance => GameManager.Instance.GameSettings;

    public bool IsInternetTime = true;
    public bool IsEnableAds = true;
    public bool IsDebugUserData = false;

    [Button, HorizontalGroup("row")]
    public void SubmitBuild()
    {
        IsInternetTime = true;
        IsEnableAds = true;
        IsDebugUserData = false;
    }

    [Button, HorizontalGroup("row")]
    public void TestBuild()
    {
        IsInternetTime = false;
        IsEnableAds = false;
        IsDebugUserData = true;
    }
}
