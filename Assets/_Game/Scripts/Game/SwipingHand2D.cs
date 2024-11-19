using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SwipingHand2D : MonoBehaviour
{
    [SerializeField] bool isLocal = true;
    [SerializeField] Transform target;
    [SerializeField] Vector3 from;
    [SerializeField] Vector3 to;
    [SerializeField] float speed = 1;
    [SerializeField] Ease ease;
    [SerializeField] float delay = 1;

    bool isEnable;
    Coroutine coroutine;

    [Button]
    public void Move()
    {
        if (!isEnable) return;
        coroutine = ScheduleUtils.DelayTask(delay, () =>
        {
            if (!isEnable) return;
            gameObject.SetActive(true);
            float duration = Vector3.Distance(from, to) / speed;
            if (isLocal)
            {
                target.localPosition = from;
                target.DOLocalMove(to, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                target.position = from;
                target.DOMove(to, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
            }
        });
    }

    public void Stop()
    {
        if (!isEnable) return;
        if (coroutine != null) ScheduleUtils.StopCoroutine(coroutine);
        target.DOKill();
        gameObject.SetActive(false);
    }

    [Button]
    public void Enable()
    {
        isEnable = true;
    }

    public void Disable()
    {
        Stop();
        isEnable = false;
    }
}
