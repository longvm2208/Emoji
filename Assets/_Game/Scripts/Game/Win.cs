using UnityEngine;

public class Win : MonoBehaviour
{
    public void Trigger()
    {
        UIManager.Instance.OpenPopup(PopupId.Win);
    }
}
