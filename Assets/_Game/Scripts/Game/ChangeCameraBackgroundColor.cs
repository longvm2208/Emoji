using UnityEngine;

public class ChangeCameraBackgroundColor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Color color;

    public void Change()
    {
        cam.backgroundColor = color;
    }
}
