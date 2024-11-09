public class PanelGame : PanelBase
{
    public void OnClickHome()
    {
        UIManager.Instance.OpenPopup(PopupId.BackHome);
    }
}
