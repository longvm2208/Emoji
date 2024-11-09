public class PopupBackHome : PopupBase
{
	#region UI EVENTS
	public void OnClickYes()
	{
		LoadSceneManager.Instance.LoadScene(SceneId.Home);
	}

	public void OnClickNo()
	{
		Close();
	}
	#endregion
}
