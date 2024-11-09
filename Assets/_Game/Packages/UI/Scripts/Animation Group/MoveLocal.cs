using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class MoveLocal : AnimationBase
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    [SerializeField] RectTransform rectTransform;

    public override void Prepare()
    {
        rectTransform.localPosition = start;
    }

    public override void Play()
    {
        rectTransform.DOLocalMove(end, duration);
    }
}