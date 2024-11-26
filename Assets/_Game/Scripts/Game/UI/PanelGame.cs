using UnityEngine;

public class PanelGame : PanelBase
{
    public float startTime;

    private void Start()
    {
        FirebaseManager.Instance.level_start(GameData.Instance.SelectedLevelIndex);
        startTime = Time.realtimeSinceStartup;
    }

    public void OnClickHome()
    {
        UIManager.Instance.OpenPopup(PopupId.BackHome);
    }
}
