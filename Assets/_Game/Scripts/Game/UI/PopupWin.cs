using DG.Tweening;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [SerializeField] RectTransform mrecPoint;
    [SerializeField] Image screenshotImage;
    [SerializeField] RectTransform screenshotRt;
    [SerializeField] RectTransform photoRt;
    [SerializeField] RectTransform maskRt;

    public override void Open(object args = null)
    {
        base.Open(args);
        VibrationManager.Instance.Vibrate();
        //screenshotImage.sprite = ScreenshotToSprite.Instance.ScreenshotSprite;
        //if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
        //{
        //    FirebaseManager.Instance.level_checkpoint(GameData.Instance.SelectedLevelIndex);
        //    GameData.Instance.CurrentLevelIndex++;
        //}
        //float baseRatio = 9f / 16;
        //Vector2 canvasSizeDelta = GameManager.Instance.CanvasSizeDelta;
        //float currentRatio = canvasSizeDelta.x / canvasSizeDelta.y;
        //Vector2 screenshotSizeDelta = screenshotRt.sizeDelta;
        //screenshotSizeDelta.y *= baseRatio / currentRatio;
        //screenshotRt.sizeDelta = canvasSizeDelta;
        //photoRt.sizeDelta = canvasSizeDelta;
        //maskRt.sizeDelta = canvasSizeDelta;
        //screenshotRt.ChangeAnchorPosY(0);
        //photoRt.ChangeAnchorPosY(0);
        //maskRt.ChangeAnchorPosY(0);
        //ScheduleUtils.DelayTask(0.25f, () =>
        //{
        //    screenshotRt.DOAnchorPosY(64, 0.75f);
        //    screenshotRt.DOSizeDelta(screenshotSizeDelta, 0.75f);
        //    photoRt.DOAnchorPosY(125, 0.75f);
        //    photoRt.DOSizeDelta(new Vector2(644, 739), 0.75f);
        //    maskRt.DOAnchorPosY(38, 0.75f);
        //    maskRt.DOSizeDelta(new Vector2(590, 570), 0.75f);
        //});
        //MaxManager.Instance.ShowMRec(MaxSdkBase.AdViewPosition.BottomCenter);
        MaxManager.Instance.ShowMRecAboveBanner();
    }

    public void OnClickRestart()
    {
        PanelGame panelGame = UIManager.Instance.GetPanel<PanelGame>();
        float duration = Time.realtimeSinceStartup - panelGame.startTime;
        FirebaseManager.Instance.level_complete(GameData.Instance.SelectedLevelIndex, (int)duration);

        MaxManager.Instance.ShowInterstitial("popup_end_card_button_restart", () =>
        {
            LoadSceneManager.Instance.ReloadCurrentScene();
        });

        MaxManager.Instance.HideMRec();
    }

    public void OnClickNext()
    {
        PanelGame panelGame = UIManager.Instance.GetPanel<PanelGame>();
        float duration = Time.realtimeSinceStartup - panelGame.startTime;
        FirebaseManager.Instance.level_complete(GameData.Instance.SelectedLevelIndex, (int)duration);

        MaxManager.Instance.ShowInterstitial("popup_end_card_button_next", () =>
        {
            if (GameData.Instance.SelectedLevelIndex < ConfigManager.Instance.LevelAmount - 1)
            {
                GameData.Instance.SelectedLevelIndex++;
            }
            LoadSceneManager.Instance.LoadSceneLevel(GameData.Instance.SelectedLevelIndex);
        });

        MaxManager.Instance.HideMRec();
    }
}
