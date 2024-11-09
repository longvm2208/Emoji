using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class FadeSkeletonAnimation : MonoBehaviour
{
    [SerializeField] SkeletonAnimation sa;
    [SerializeField] float from;
    [SerializeField] float to;
    [SerializeField] float duration;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] UnityEvent onComplete;

    void OnValidate()
    {
        if (sa == null) sa = GetComponent<SkeletonAnimation>();
    }

    [Button]
    public void Fade()
    {
        DOVirtual.Float(from, to, duration, (value) => { sa.skeleton.A = value; }).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }
}
