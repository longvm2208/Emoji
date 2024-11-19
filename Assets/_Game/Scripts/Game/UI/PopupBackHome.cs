public class PopupBackHome : PopupBase
{
	#region UI EVENTS
	public void OnClickYes()
	{
		MaxManager.Instance.ShowInterstitial("popup_back_home_button_yes", () =>
		{
            LoadSceneManager.Instance.LoadScene(SceneId.Home);
        });
	}

	public void OnClickNo()
	{
		MaxManager.Instance.ShowInterstitial("popup_back_home_button_no", () =>
		{
            Close();
        });
	}
	#endregion
}
