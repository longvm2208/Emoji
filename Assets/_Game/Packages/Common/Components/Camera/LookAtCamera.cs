using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum Mode
    {
        LookAt,
        LookAtInverted,
    }

    [SerializeField] Transform myTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Mode mode = Mode.LookAt;

    Vector3 direction;

    void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                myTransform.LookAt(cameraTransform);
                break;
            case Mode.LookAtInverted:
                direction = myTransform.position - cameraTransform.position;
                myTransform.LookAt(myTransform.position + direction);
                break;
        }
    }
}
