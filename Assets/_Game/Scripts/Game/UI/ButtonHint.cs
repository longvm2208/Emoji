using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHint : SingletonMonoBehaviour<ButtonHint>
{
    [SerializeField] GameObject adGo;
    [SerializeField] ItemBar itemBar;
    [SerializeField] BigItem[] items;

    public bool IsHint { get; private set; }

    private void Start()
    {
        gameObject.SetActive(GameData.Instance.SelectedLevelIndex > 0);
        adGo.SetActive(GameData.Instance.HintsCount == 0);
    }

    void Hint()
    {
        IsHint = true;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject.activeSelf)
            {
                items[i].Hint();
                return;
            }
        }

        itemBar.Hint();
    }

    public void OnSmallItemSelected()
    {
        StartCoroutine(OnSmallItemSelectedCoroutine());
    }

    IEnumerator OnSmallItemSelectedCoroutine()
    {
        yield return null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject.activeSelf)
            {
                items[i].Hint();
                yield break;
            }
        }
        IsHint = false;
    }

    public void OnHintCompleted()
    {
        StartCoroutine(OnHintCompletedCoroutine());
    }

    IEnumerator OnHintCompletedCoroutine()
    {
        yield return null;
        IsHint = false;
    }

    bool CanHint()
    {
        if (IsHint) return false;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject.activeSelf && items[i].CanHint())
            {
                return true;
            }
        }

        return itemBar.CanHint();
    }

    public void OnClick()
    {
        if (!CanHint()) return;
        if (GameData.Instance.HintsCount > 0)
        {
            GameData.Instance.HintsCount--;
            adGo.SetActive(GameData.Instance.HintsCount == 0);
            Hint();
        }
        else
        {
            MaxManager.Instance.ShowRewardedAd("ingame_hint", () =>
            {
                Hint();
            });
        }
    }
}
