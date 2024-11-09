using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    public void OnClick()
    {
        LoadSceneManager.Instance.LoadSceneLevel(GameData.Instance.SelectedLevelIndex);
    }
}
