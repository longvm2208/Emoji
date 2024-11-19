using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MovingHand2D : MonoBehaviour
{
    [SerializeField] Transform moving;
    [SerializeField] Transform end;
    [SerializeField] float speed;
    [SerializeField] float delay;
    [SerializeField] float stop = 0.25f;
    [SerializeField] Ease ease = Ease.Linear;

    [SerializeField] bool isEnable;
    bool isMoving;
    IEnumerator coroutine;

    public void Move()
    {
        if (!isEnable) return;
        if (isMoving) return;
        isMoving = true;
        if (coroutine == null) coroutine = MoveRoutine();
        CoroutineManager.Instance.StartCoroutine(coroutine);
    }

    IEnumerator MoveRoutine()
    {
        float duration = Vector3.Distance(moving.position, end.position) / speed;
        while (true)
        {
            yield return WaitForUtils.Seconds(delay);
            gameObject.SetActive(true);
            Vector3 position = moving.localPosition;
            moving.DOMove(end.position, duration).SetEase(ease);
            yield return WaitForUtils.Seconds(duration + stop);
            gameObject.SetActive(false);
            moving.localPosition = position;
        }
    }

    public void Stop()
    {
        if (!isEnable) return;
        if (!isMoving) return;
        isMoving = false;
        if (coroutine != null) CoroutineManager.Instance.StopCoroutine(coroutine);
        moving.DOKill();
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        Stop();
        isEnable = false;
    }

    public void Enable()
    {
        isEnable = true;
    }
}
