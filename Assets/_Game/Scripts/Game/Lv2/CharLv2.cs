using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class CharLv2 : MonoBehaviour
{
    [SerializeField] SkeletonAnimation anim;
    [SerializeField, SpineAnimation(dataField = "anim")] string body4;
    [SerializeField, SpineAnimation(dataField = "anim")] string eyes1;
    [SerializeField, SpineAnimation(dataField = "anim")] string eyes2;
    [SerializeField, SpineAnimation(dataField = "anim")] string eyes34;
    [SerializeField, SpineAnimation(dataField = "anim")] string mouth12;
    [SerializeField, SpineAnimation(dataField = "anim")] string mouth3;
    [SerializeField, SpineAnimation(dataField = "anim")] string mouth4;

    int foamCount;

    [SerializeField] UnityEvent onStep2Complete;

    private void Start()
    {
        anim.AnimationState.SetAnimation(0, eyes1, true);
        anim.AnimationState.SetAnimation(1, mouth12, false);
    }

    public void Step1()
    {
        anim.AnimationState.SetAnimation(0, eyes2, true);
    }

    public void Step3()
    {
        anim.AnimationState.SetAnimation(1, mouth3, false);
    }

    public void Step4()
    {
        anim.AnimationState.SetAnimation(0, body4, true);
        anim.AnimationState.SetAnimation(1, mouth4, true);
    }

    public void OnFoam()
    {
        foamCount++;
        if (foamCount >= 5)
        {
            onStep2Complete?.Invoke();
        }
    }
}
