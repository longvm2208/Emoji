using UnityEngine;

public class ScaleBackground : MonoBehaviour
{
    [SerializeField] Transform myTransform;

    private void Start()
    {
        float baseRatio = 9f / 16;
        float currentRatio = (float)Screen.width / Screen.height;
        float scale;
        if (baseRatio < currentRatio) scale = currentRatio / baseRatio;
        else scale = baseRatio / currentRatio;
        myTransform.localScale *= scale;
    }
}
