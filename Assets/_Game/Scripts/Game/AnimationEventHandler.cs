using Spine.Unity;
using Spine;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] SkeletonAnimation sa;
    [SerializeField, SpineEvent(dataField: "sa")] string eventName;
    [SerializeField] UnityEvent onEvent;

    EventData eventData;

    void OnValidate()
    {
        if (sa == null) sa = GetComponent<SkeletonAnimation>();
    }

    void Start()
    {
        eventData = sa.Skeleton.Data.FindEvent(eventName);
        sa.AnimationState.Event += HandleAnimationStateEvent;
    }

    void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (eventData == e.Data)
        {
            onEvent?.Invoke();
        }
    }
}
