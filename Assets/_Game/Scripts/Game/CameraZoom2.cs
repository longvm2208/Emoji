using UnityEngine;

public class CameraZoom2 : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Start()
    {
        float baseRatio = 9f / 16;
        float currentRatio = (float)Screen.width / Screen.height;
        if (currentRatio < baseRatio)
        {
            cam.orthographicSize *= baseRatio / currentRatio;
        }
    }
}
