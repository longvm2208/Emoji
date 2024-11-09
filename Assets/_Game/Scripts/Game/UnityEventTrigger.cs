using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] UnityEvent onEvent;

    public void Trigger() => ScheduleUtils.DelayTask(delay, () => onEvent?.Invoke());
}
