using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class CharLv3 : MonoBehaviour
{
    [SerializeField] SkeletonAnimation anim;
    [SerializeField, SpineAnimation(dataField = "anim")] string body1;
    [SerializeField, SpineAnimation(dataField = "anim")] string body2;
    [SerializeField, SpineAnimation(dataField = "anim")] string mounth1;
    [SerializeField, SpineAnimation(dataField = "anim")] string mounth2;
    [SerializeField, SpineAnimation(dataField = "anim")] string mounth3;
    [SerializeField, SpineAnimation(dataField = "anim")] string eye1;
    [SerializeField, SpineAnimation(dataField = "anim")] string eye2;
    [SerializeField, SpineAnimation(dataField = "anim")] string eye3;

    [SerializeField] float[] seconds;
    [SerializeField] UnityEvent[] RutThanhSat;
    [SerializeField] UnityEvent OnStep1Complete;
    [SerializeField] UnityEvent OnStep2Complete;

    TrackEntry body1Te;

    private void Start()
    {
        body1Te = anim.AnimationState.SetAnimation(0, body1, false);
        body1Te.TimeScale = 0;
        anim.AnimationState.SetAnimation(1, eye1, true);
        anim.AnimationState.SetAnimation(2, mounth1, true);
    }

    public void Step1()
    {
        body1Te.TimeScale = 0.5f;
        Debug.Log("sound");
        ScheduleUtils.DelayTask(seconds[0], () =>
        {
            Debug.Log("sound");
            RutThanhSat[0]?.Invoke();
            ScheduleUtils.DelayTask(seconds[1], () =>
            {
                Debug.Log("sound");
                RutThanhSat[1]?.Invoke();
                ScheduleUtils.DelayTask(seconds[2], () =>
                {
                    Debug.Log("sound");
                    RutThanhSat[2]?.Invoke();
                });
            });
        });
        ScheduleUtils.DelayTask(2.333f * 2, () =>
        {
            OnStep1Complete?.Invoke();
        });
    }

    public void Step2()
    {
        anim.AnimationState.SetAnimation(0, body2, false);
        ScheduleUtils.DelayTask(2.7f, () =>
        {
            anim.AnimationState.ClearTrack(0);
            anim.AnimationState.ClearTrack(2);
            anim.Skeleton.SetToSetupPose();
            anim.AnimationState.SetAnimation(0, eye2, true);
            anim.AnimationState.SetAnimation(1, mounth2, true);
            OnStep2Complete?.Invoke();
        });
    }
}
