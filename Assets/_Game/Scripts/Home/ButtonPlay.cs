using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    public void OnClick()
    {
        MaxManager.Instance.ShowInterstitial("home_button_play", () =>
        {
            LoadSceneManager.Instance.LoadSceneLevel(GameData.Instance.SelectedLevelIndex);
            MaxManager.Instance.HideMRec();
        });
    }
}
