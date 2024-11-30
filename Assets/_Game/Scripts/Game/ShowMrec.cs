using UnityEngine;

public class ShowMrec : MonoBehaviour
{
    public void Show()
    {
        Debug.Log("show mrec");
        MaxManager.Instance.ShowMRec(MaxSdkBase.AdViewPosition.BottomCenter);
    }

    public void Hide()
    {
        Debug.Log("hide mrec");
        MaxManager.Instance.HideMRec();
    }
}
