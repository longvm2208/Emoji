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
        int levelIndex = GameData.Instance.SelectedLevelIndex;
        int swappedIndex = ConfigManager.Instance.Levels[levelIndex] - 1;
        levels[swappedIndex].SetActive(true);
        ShowMrec();

        AudioManager.Instance.ChangeMusicVolume(1);
    }

    public void ShowMrec()
    {
        //MaxManager.Instance.ShowMRec(mrecPoint);
        MaxManager.Instance.ShowMRecAboveBanner();
    }
}
