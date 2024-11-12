using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SetAnimation : MonoBehaviour
{
    [SerializeField] SkeletonAnimation sa;
    [SerializeField] int trackIndex;
    [SerializeField, SpineAnimation(dataField = "sa")] string anim;
    [SerializeField] bool loop;
    [SerializeField] float timeScale = 1;
    [SerializeField] UnityEvent onComplete;

    void OnValidate()
    {
        if (sa == null) sa = GetComponent<SkeletonAnimation>();
    }

    public void Set()
    {
        sa.AnimationState.SetAnimation(trackIndex, anim, loop).TimeScale = timeScale;
        CoroutineManager.Instance.StartCoroutine(CheckCompleteRoutine());
    }

    IEnumerator CheckCompleteRoutine()
    {
        yield return new WaitUntil(() => IsComplete());
        onComplete?.Invoke();
    }

    bool IsComplete()
    {
        var trackEntry = sa.AnimationState.GetCurrent(trackIndex);
        return trackEntry.AnimationTime >= trackEntry.AnimationEnd;
    }
}
