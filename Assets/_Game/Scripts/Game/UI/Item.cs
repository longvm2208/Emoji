using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] ItemBar itemBar;
    [SerializeField] RectTransform myRt;
    public RectTransform MyRt => myRt;
    [SerializeField] Button button;
    [SerializeField] Image bgImage;
    [SerializeField] Image iconImage;
    [SerializeField, ExposedScriptableObject] ItemConfig config;
    [SerializeField] UnityEvent onClick;

    void OnValidate()
    {
        if (myRt == null) myRt = transform as RectTransform;
    }

    public void Reload(int status)
    {
        bgImage.sprite = config.BgSprites[status];
        button.interactable = status == 1;
        if (status == 2)
        {
            iconImage.sprite = config.TickSprite;
            iconImage.SetNativeSize();
        }
    }

    #region UI EVENTS
    public void OnClick()
    {
        button.interactable = false;
        itemBar.OnItemSelected();
        onClick?.Invoke();
    }
    #endregion
}
