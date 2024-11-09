using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 endPosition;
    [SerializeField] float duration;
    [SerializeField] bool isLocal;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] UnityEvent onComplete;

    public void Move()
    {
        if (isLocal)
        {
            target.DOLocalMove(endPosition, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            target.DOMove(endPosition, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }
    }
}
