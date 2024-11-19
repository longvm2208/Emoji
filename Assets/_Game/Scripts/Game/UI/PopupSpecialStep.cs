using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupSpecialStep : PopupBase
{
    [SerializeField] Image screenshotImage;
    [SerializeField] RectTransform screenshotRt;
    [SerializeField] RectTransform photoRt;
    [SerializeField] RectTransform maskRt;
    [SerializeField] GameObject tapToCloseGo;
	[SerializeField] UnityEvent onClickSpecial;

    private void OnEnable()
    {
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
        maskRt.ChangeAnchorPosY(0);
        ScheduleUtils.DelayTask(0.25f, () =>
        {
            screenshotRt.DOAnchorPosY(64, 0.75f);
            screenshotRt.DOSizeDelta(screenshotSizeDelta, 0.75f);
            photoRt.DOAnchorPosY(125, 0.75f);
            photoRt.DOSizeDelta(new Vector2(644, 739), 0.75f);
            maskRt.DOAnchorPosY(38, 0.75f);
            maskRt.DOSizeDelta(new Vector2(590, 570), 0.75f);
            ScheduleUtils.DelayTask(2, () =>
            {
                tapToCloseGo.SetActive(true);
            });
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
        MaxManager.Instance.ShowInterstitial("popup_special_step_button_restart", () =>
        {
            if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
            {
                GameData.Instance.CurrentLevelIndex++;
            }
            LoadSceneManager.Instance.ReloadCurrentScene();
        });
    }

    public void OnClickSpecial()
	{
        MaxManager.Instance.ShowRewardedAd("special_step", () =>
        {
            onClickSpecial?.Invoke();
            Close();
        });
	}

	public void OnClickClose()
	{
        MaxManager.Instance.ShowInterstitial("popup_special_step_button_close", () =>
        {
            if (GameData.Instance.CurrentLevelIndex == GameData.Instance.SelectedLevelIndex)
            {
                GameData.Instance.CurrentLevelIndex++;
            }
            if (GameData.Instance.SelectedLevelIndex < ConfigManager.Instance.LevelAmount - 1)
            {
                GameData.Instance.SelectedLevelIndex++;
            }
            LoadSceneManager.Instance.LoadScene(SceneId.Home);
        });
    }
    #endregion
}
