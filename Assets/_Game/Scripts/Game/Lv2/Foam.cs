using UnityEngine;
using UnityEngine.Events;

public class Foam : MonoBehaviour
{
    [SerializeField] Collider2D canCollider;
    [SerializeField] UnityEvent onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCollider == collision) onTriggerEnter?.Invoke();
    }
}
