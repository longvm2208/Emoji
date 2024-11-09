using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FadeSpriteRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float alpha;
    [SerializeField] float duration;
    [SerializeField] UnityEvent onComplete;

    void OnValidate()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    public void Fade()
    {
        sr.DoFade(sr.color.a, alpha, duration).OnComplete(() => onComplete?.Invoke());
    }
}
