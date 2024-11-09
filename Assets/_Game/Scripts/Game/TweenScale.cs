using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TweenScale : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float scale;
    [SerializeField] float duration;
    [SerializeField] Ease ease = Ease.OutBack;
    [SerializeField] UnityEvent onComplete;

    public void Tween()
    {
        target.DOScale(scale, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }
}
