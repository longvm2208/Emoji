using UnityEngine;

public class PositionChanger : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 position;
    [SerializeField] bool isLocal;

    public void Change()
    {
        if (isLocal)
        {
            target.localPosition = position;
        }
        else
        {
            target.position = position;
        }
    }
}
