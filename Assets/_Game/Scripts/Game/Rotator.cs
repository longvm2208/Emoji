using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Rotator : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 endRotation;
    [SerializeField] float duration;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] UnityEvent onComplete;

    public void Rotate()
    {
        target.DORotate(endRotation, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }
}
