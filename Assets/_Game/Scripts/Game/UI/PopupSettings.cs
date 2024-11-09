using System.Collections;
using System.Collections.Generic;

public class PopupSettings : PopupBase
{
    public override void Open(object args = null)
    {
        base.Open(args);
    }

    #region UI EVENTS
    public void OnClickClose()
	{
		Close();
	}
	#endregion
}
