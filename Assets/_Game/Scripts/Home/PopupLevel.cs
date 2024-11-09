using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLevel : PopupBase
{
	[SerializeField] LevelOptionUI[] levelOptions;

    public override void Open(object args = null)
    {
        base.Open(args);
		for (int i = 0; i < levelOptions.Length; i++)
		{
			levelOptions[i].Init(i);
		}
    }

    #region UI EVENTS
    public void OnClickBack()
	{
		Close();
	}
	#endregion
}
