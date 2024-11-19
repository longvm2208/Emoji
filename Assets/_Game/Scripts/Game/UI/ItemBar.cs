using DG.Tweening;
using UnityEngine;

public class ItemBar : MonoBehaviour
{
    [SerializeField] float positionShake = 0.25f;
    [SerializeField] float rotationShake = 1;
    [SerializeField] float showPosY;
    [SerializeField] float hidePosY;
    [SerializeField] float duration;
    [SerializeField] GameObject fireworkGo;
    [SerializeField] GameObject tickGo;
    [SerializeField] RectTransform myRt;
    [SerializeField] RectTransform handRt;
    [SerializeField] ButtonHint buttonHint;
    [SerializeField] Item[] items;

    bool isHint;
    bool isShowing = true;
    int currentId;

    void Start()
    {
        currentId = 0;
        for (int i = 0; i < items.Length; i++)
        {
            int status = 0;
            if (i == 0) status = 1;
            items[i].Reload(status);
        }
    }

    public void OnItemSelected()
    {
        if (isHint)
        {
            buttonHint.OnSmallItemSelected();
        }
        isHint = false;
        handRt.gameObject.SetActive(false);
        myRt.DOAnchorPosY(hidePosY, duration);
        isShowing = false;
    }

    public void NextItem()
    {
        if (currentId == 0)
        {
            if (CameraShake.HasInstance)
            {
                CameraShake.Instance.ShakePosition(0.15f, positionShake);
                CameraShake.Instance.ShakeRotation(0.15f, rotationShake);
            }
        }
        VibrationManager.Instance.Vibrate();
        fireworkGo.SetActive(false);
        fireworkGo.SetActive(true);
        tickGo.SetActive(true);
        ScheduleUtils.DelayTask(2, () =>
        {
            tickGo.SetActive(false);
            items[currentId].Reload(2);
            currentId++;
            items[currentId].Reload(1);
            isShowing = true;
            myRt.DOAnchorPosY(showPosY, duration);
            if (GameData.Instance.SelectedLevelIndex == 0 || ButtonHint.Instance.IsHint)
            {
                handRt.gameObject.SetActive(true);
                handRt.position = items[currentId].MyRt.position;
            }
        });
    }

    public void Hint()
    {
        isHint = true;
        handRt.gameObject.SetActive(true);
        handRt.position = items[currentId].MyRt.position;
    }

    public bool CanHint()
    {
        return isShowing && !isHint;
    }

    public void Firework()
    {
        VibrationManager.Instance.Vibrate();
        fireworkGo.SetActive(false);
        fireworkGo.SetActive(true);
        tickGo.SetActive(true);
        ScheduleUtils.DelayTask(2, () =>
        {
            tickGo.SetActive(false);
        });
    }

    public void Hide()
    {
        isShowing = false;
        myRt.DOAnchorPosY(hidePosY, duration);
    }
}
