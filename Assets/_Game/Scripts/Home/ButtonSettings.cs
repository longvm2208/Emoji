using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.OpenPopup(PopupId.Settings);
    }
}
