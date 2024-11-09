using Spine;
using Spine.Unity;
using UnityEngine;

public class ChangeSkeletonAnimationAlpha : MonoBehaviour
{
    [SerializeField] SkeletonAnimation sa;
    [SerializeField] float a;

    void OnValidate()
    {
        if (sa == null) sa = GetComponent<SkeletonAnimation>();
    }

    public void Change()
    {
        Skeleton skeleton = sa.skeleton;
        skeleton.A = a;
    }
}
