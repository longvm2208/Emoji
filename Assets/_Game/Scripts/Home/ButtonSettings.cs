using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
    public void OnClick()
    {
        MaxManager.Instance.ShowInterstitial("home_button_settings", () =>
        {
            UIManager.Instance.OpenPopup(PopupId.Settings);
        });
    }
}
