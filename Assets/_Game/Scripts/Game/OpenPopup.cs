using UnityEngine;

public class OpenPopup : MonoBehaviour
{
    [SerializeField] PopupId id;

    public void Open()
    {
        UIManager.Instance.OpenPopup(id);
    }
}
