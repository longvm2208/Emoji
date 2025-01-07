using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupSpecialStep : PopupBase
{
    [SerializeField] GameObject adGo;
    [SerializeField] Image screenshotImage;
    [SerializeField] RectTransform screenshotRt;
    [SerializeField] RectTransform photoRt;
    [SerializeField] RectTransform maskRt;
    [SerializeField] GameObject tapToCloseGo;
	[SerializeField] UnityEvent onClickSpecial;

    private void OnEnable()
    {
        adGo.SetActive(DataManager.Instance.GameData.SelectedLevelIndex > 0);
        VibrationManager.Instance.Vibrate();
        tapToCloseGo.SetActive(false);
        screenshotImage.sprite = ScreenshotToSprite.Instance.ScreenshotSprite;
        float baseRatio = 9f / 16;
        Vector2 canvasSizeDelta = GameManager.Instance.CanvasSizeDelta;
        float currentRatio = canvasSizeDelta.x / canvasSizeDelta.y;
        Vector2 screenshotSizeDelta = screenshotRt.sizeDelta;
        screenshotSizeDelta.y *= baseRatio / currentRatio;
        screenshotRt.sizeDelta = canvasSizeDelta;
        photoRt.sizeDelta = canvasSizeDelta;
        maskRt.sizeDelta = canvasSizeDelta;
        screenshotRt.ChangeAnchorPosY(0);
        photoRt.ChangeAnchorPosY(0);
        photoRt.localScale = Vector3.one;
        maskRt.ChangeAnchorPosY(0);
        ScheduleUtils.DelayTask(0.25f, () =>
        {
            screenshotRt.DOAnchorPosY(64, 0.75f);
            screenshotRt.DOSizeDelta(screenshotSizeDelta, 0.75f);
            photoRt.DOAnchorPosY(386, 0.75f);
            photoRt.DOSizeDelta(new Vector2(644, 739), 0.75f);
            photoRt.DOScale(0.8f, 0.75f);
            maskRt.DOAnchorPosY(38, 0.75f);
            maskRt.DOSizeDelta(new Vector2(590, 570), 0.75f);
            //ScheduleUtils.DelayTask(2, () =>
            //{
            //    tapToCloseGo.SetActive(true);
            //});
            MaxManager.Instance.ShowMRecAboveBanner();
        });
    }

    public override void Open(object args = null)
    {
        base.Open(args);
		tapToCloseGo.SetActive(false);
		ScheduleUtils.DelayTask(2, () =>
		{
			tapToCloseGo.SetActive(true);
		});
    }

    #region UI EVENTS
	public void OnClickRestart()
	{
        PanelGame panelGame = UIManager.Instance.GetPanel<PanelGame>();
        float duration = Time.realtimeSinceStartup - panelGame.startTime;
        FirebaseManager.Instance.level_complete(GameData.Instance.SelectedLevelIndex, (int)duration);

        MaxManager.Instance.ShowInterstitial("popup_special_step_button_restart", () =>
        {
            if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
            {
                FirebaseManager.Instance.level_checkpoint(GameData.Instance.SelectedLevelIndex);
                GameData.Instance.CurrentLevelIndex++;
            }
            LoadSceneManager.Instance.ReloadCurrentScene();
        });

        MaxManager.Instance.HideMRec();
    }

    public void OnClickSpecial()
	{
        if (GameData.Instance.SelectedLevelIndex == 0)
        {
            onClickSpecial?.Invoke();
            Close();
        }
        else
        {
            MaxManager.Instance.ShowRewardedAd("special_step", () =>
            {
                onClickSpecial?.Invoke();
                Close();
            });
        }
	}

	//public void OnClickClose()
	//{
 //       PanelGame panelGame = UIManager.Instance.GetPanel<PanelGame>();
 //       float duration = Time.realtimeSinceStartup - panelGame.startTime;
 //       FirebaseManager.Instance.level_complete(GameData.Instance.SelectedLevelIndex, (int)duration);

 //       MaxManager.Instance.ShowInterstitial("popup_special_step_button_close", () =>
 //       {
 //           if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
 //           {
 //               FirebaseManager.Instance.level_checkpoint(GameData.Instance.SelectedLevelIndex);
 //               GameData.Instance.CurrentLevelIndex++;
 //           }
 //           if (GameData.Instance.SelectedLevelIndex < ConfigManager.Instance.LevelAmount - 1)
 //           {
 //               GameData.Instance.SelectedLevelIndex++;
 //           }
 //           LoadSceneManager.Instance.LoadSceneLevel(GameData.Instance.SelectedLevelIndex);
 //       });
 //   }

    public void OnClickNext()
    {
        PanelGame panelGame = UIManager.Instance.GetPanel<PanelGame>();
        float duration = Time.realtimeSinceStartup - panelGame.startTime;
        FirebaseManager.Instance.level_complete(GameData.Instance.SelectedLevelIndex, (int)duration);

        MaxManager.Instance.ShowInterstitial("popup_end_card_button_next", () =>
        {
            if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
            {
                FirebaseManager.Instance.level_checkpoint(GameData.Instance.SelectedLevelIndex);
                GameData.Instance.CurrentLevelIndex++;
            }
            if (GameData.Instance.SelectedLevelIndex < ConfigManager.Instance.LevelAmount - 1)
            {
                GameData.Instance.SelectedLevelIndex++;
            }
            LoadSceneManager.Instance.LoadSceneLevel(GameData.Instance.SelectedLevelIndex);
        });

        MaxManager.Instance.HideMRec();
    }
    #endregion
}
