using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FadeSpriteRenderers : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] srs;
    [SerializeField] float alpha;
    [SerializeField] float duration;
    [SerializeField] UnityEvent onComplete;

    public void Fade()
    {
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].DoFade(srs[i].color.a, alpha, duration).OnComplete(() => onComplete?.Invoke());
        }
    }
}
