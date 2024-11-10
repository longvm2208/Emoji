using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevel : PopupBase
{
	[SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentRt;
    [SerializeField] LevelOptionUI[] levelOptions;

    public override void Open(object args = null)
    {
        base.Open(args);
		for (int i = 0; i < levelOptions.Length; i++)
		{
			levelOptions[i].Init(i);
		}
        RectTransform targetRt = levelOptions[GameData.Instance.SelectedLevelIndex].MyRt;
        StartCoroutine(ScrollSnapRoutine(targetRt));
    }

    IEnumerator ScrollSnapRoutine(RectTransform itemRt)
    {
        yield return null;
        float contentHeight = contentRt.rect.height;
        float itemHeight = itemRt.rect.height;
        float itemYPosition = -itemRt.anchoredPosition.y;
        float normalizedPos = Mathf.Clamp01((itemYPosition - itemHeight / 2) / (contentHeight - scrollRect.viewport.rect.height));
        scrollRect.DOKill();
        scrollRect.DONormalizedPos(new Vector2(0, 1 - normalizedPos + 0.025f), 0.5f);
    }

    #region UI EVENTS
    public void OnClickBack()
	{
		Close();
	}
	#endregion
}
