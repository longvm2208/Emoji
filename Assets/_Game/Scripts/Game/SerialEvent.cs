using UnityEngine;
using UnityEngine.Events;

public class SerialEvent : MonoBehaviour
{
    [SerializeField] UnityEvent[] onEvents;

    int i = 0;

    public void Trigger()
    {
        onEvents[i]?.Invoke();
        i++;
    }
}
