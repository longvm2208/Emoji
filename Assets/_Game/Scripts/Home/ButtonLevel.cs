using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    public void OnClick()
    {
        MaxManager.Instance.ShowInterstitial("home_button_menu", () =>
        {
            UIManager.Instance.OpenPopup(PopupId.Level);
        });
    }
}
