using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BigItem : MonoBehaviour
{
    [SerializeField] UnityEvent onHint;

    bool checkCollisionCompleted;

    public void Hint()
    {
        if (checkCollisionCompleted) return;

        onHint?.Invoke();
    }

    public void OnCheckCollisionCompleted()
    {
        checkCollisionCompleted = true;
        ButtonHint.Instance.OnHintCompleted();
    }

    public bool CanHint()
    {
        return !checkCollisionCompleted;
    }

    [Button]
    public void CopyFromOnGoEnable()
    {
        OnGoEnable onGoEnable = GetComponent<OnGoEnable>();
        onHint = onGoEnable.OnEnableUE;
    }
}
