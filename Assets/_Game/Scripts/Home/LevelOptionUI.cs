using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelOptionUI : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] RectTransform myRt;
    public RectTransform MyRt => myRt;
    [SerializeField] Image bgImage;
    [SerializeField] TMP_Text indexText;
    [SerializeField] Sprite[] bgSprites;
    [SerializeField] AnimationCurve animationCurve;

    int index;

    private void OnDisable()
    {
        if (index == GameData.Instance.SelectedLevelIndex)
        {
            myRt.DOKill();
            myRt.localScale = Vector3.one;
        }
    }

    public void Init(int index)
    {
        this.index = index;
        indexText.text = (index + 1).ToString();
        if (index > GameData.Instance.CurrentLevelIndex)
        {
            button.interactable = false;
            indexText.gameObject.SetActive(false);
            bgImage.sprite = bgSprites[2];
        }
        else if (index == GameData.Instance.SelectedLevelIndex)
        {
            bgImage.sprite = bgSprites[1];
            myRt.DOScale(1.1f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            bgImage.sprite = bgSprites[0];
        }
    }

    #region UI EVENTS
    public void OnClick()
    {
        MaxManager.Instance.ShowInterstitial("popup_menu_level_option", () =>
        {
            GameData.Instance.SelectedLevelIndex = index;
            LoadSceneManager.Instance.LoadSceneLevel(index);
            MaxManager.Instance.HideMRec();
        });
    }
    #endregion
}
