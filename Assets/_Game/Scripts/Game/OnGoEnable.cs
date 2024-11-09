using UnityEngine;
using UnityEngine.Events;

public class OnGoEnable : MonoBehaviour
{
    [SerializeField] UnityEvent onEnable;

    void OnEnable()
    {
        onEnable?.Invoke();
    }
}
