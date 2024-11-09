using DG.Tweening;
using UnityEngine;

public class ItemBar : MonoBehaviour
{
    [SerializeField] float showPosY;
    [SerializeField] float hidePosY;
    [SerializeField] float duration;
    [SerializeField] GameObject fireworkGo;
    [SerializeField] GameObject tickGo;
    [SerializeField] RectTransform myRt;
    [SerializeField] RectTransform handRt;
    [SerializeField] Item[] items;

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
        handRt.gameObject.SetActive(false);
        myRt.DOAnchorPosY(hidePosY, duration);
    }

    public void NextItem()
    {
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
            handRt.gameObject.SetActive(true);
            handRt.position = items[currentId].MyRt.position;
            myRt.DOAnchorPosY(showPosY, duration);
        });
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
        myRt.DOAnchorPosY(hidePosY, duration);
    }
}
