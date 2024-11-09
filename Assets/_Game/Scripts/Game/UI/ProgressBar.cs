using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] float showPosY;
    [SerializeField] float hidePosY;
    [SerializeField] float duration;
    [SerializeField] RectTransform myRt;
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text progressTmp;

    private void OnValidate()
    {
        if (myRt == null) myRt = transform as RectTransform;
    }

    public void UpdateProgress(float progress)
    {
        fillImage.fillAmount = progress;
        //progressTmp.text = ((int)(progress * 100)).ToString();
    }

    public void Show()
    {
        myRt.DOAnchorPosY(showPosY, duration);
    }

    public void Hide()
    {
        myRt.DOAnchorPosY(hidePosY, duration);
    }
}
