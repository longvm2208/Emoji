using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Button Click Config")]
public class ButtonClickConfig : ScriptableObject
{
    [Range(0.01f, 1)] public float Duration = 0.5f;
    [Range(0.01f, 1)] public float Scale = 0.8f;
    public AnimationCurve Curve;
}