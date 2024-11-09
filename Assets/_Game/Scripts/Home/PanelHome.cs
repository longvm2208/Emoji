using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelHome : PanelBase
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] GameObject[] levels;

    private void Start()
    {
        levelText.text = $"Level {GameData.Instance.SelectedLevelIndex + 1}";
        levels[GameData.Instance.SelectedLevelIndex].SetActive(true);
    }
}
