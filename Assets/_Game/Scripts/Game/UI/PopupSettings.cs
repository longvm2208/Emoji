using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettings : PopupBase
{
    [SerializeField] RectTransform mrecPoint;

    public override void Open(object args = null)
    {
        base.Open(args);
        MaxManager.Instance.ShowMRec(mrecPoint);
    }

    #region UI EVENTS
    public void OnClickClose()
	{
        MaxManager.Instance.ShowInterstitial("popup_settings_button_close", () =>
        {
            UIManager.Instance.GetPanel<PanelHome>().ShowMrec();
            Close();
        });
	}
	#endregion
}
