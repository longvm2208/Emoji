using UnityEngine;
using UnityEngine.Events;

public class ReverseProgress : MonoBehaviour
{
    [SerializeField] UnityEvent<float> onReverse;

    public void Reverse(float progress)
    {
        progress = 1 - progress;
        progress = Mathf.Clamp01(progress);
        onReverse?.Invoke(progress);
    }
}