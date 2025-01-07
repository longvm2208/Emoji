using UnityEngine;

public class PanelGame : PanelBase
{
    public float startTime;

    private void Start()
    {
        FirebaseManager.Instance.level_start(GameData.Instance.SelectedLevelIndex);
        startTime = Time.realtimeSinceStartup;

        if (GameData.Instance.showPopupRate && GameData.Instance.CurrentLevelIndex >= 2)
        {
            GameData.Instance.showPopupRate = false;
            UIManager.Instance.OpenPopup(PopupId.Rate);
        }

        AudioManager.Instance.ChangeMusicVolume(0.5f);
    }

    public void OnClickHome()
    {
        UIManager.Instance.OpenPopup(PopupId.BackHome);
    }
}
