using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.OpenPopup(PopupId.Level);
    }
}
