using TMPro;
using UnityEngine;

public class PanelHome : PanelBase
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] RectTransform mrecPoint;
    [SerializeField] GameObject[] levels;

    private void Start()
    {
        levelText.text = $"Level {GameData.Instance.SelectedLevelIndex + 1}";
        levels[GameData.Instance.SelectedLevelIndex].SetActive(true);
        ShowMrec();
    }

    public void ShowMrec()
    {
        MaxManager.Instance.ShowMRec(mrecPoint);
    }
}
